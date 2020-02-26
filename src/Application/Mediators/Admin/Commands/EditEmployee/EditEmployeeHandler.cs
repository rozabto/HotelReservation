using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Repositories;
using MediatR;

namespace Application.Admin.Commands.EditEmployee
{
    public class EditEmployeeHandler : IRequestHandler<EditEmployeeCommand>
    {
        private readonly IEmployeeRepository _employee;

        public EditEmployeeHandler(IEmployeeRepository employee)
        {
            _employee = employee ?? throw new ArgumentNullException(nameof(employee));
        }

        public async Task<Unit> Handle(EditEmployeeCommand request, CancellationToken cancellationToken)
        {
            if (!request.EGN.HasValue && string.IsNullOrWhiteSpace(request.MiddleName))
                return Unit.Value;

            var employee = await _employee.GetEmployeeWithUser(request.Id, cancellationToken)
                ?? throw new NotFoundException("Employee", request.Id);

            if (request.EGN.HasValue)
                employee.EGN = request.EGN.Value;

            if (!string.IsNullOrWhiteSpace(request.MiddleName))
                employee.MiddleName = request.MiddleName;

            await _employee.Update(employee, cancellationToken);

            return Unit.Value;
        }
    }
}
