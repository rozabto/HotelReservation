using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Repositories;
using Common;
using MediatR;

namespace Application.Data.Commands.EmployeeActivity
{
    public class EmployeeActivityHandler : IRequestHandler<EmployeeActivityCommand>
    {
        private readonly IEmployeeRepository _employee;
        private readonly IDateTime _date;

        public EmployeeActivityHandler(IEmployeeRepository employee, IDateTime date)
        {
            _employee = employee ?? throw new ArgumentNullException(nameof(employee));
            _date = date ?? throw new ArgumentNullException(nameof(date));
        }

        public async Task<Unit> Handle(EmployeeActivityCommand request, CancellationToken cancellationToken)
        {
            await _employee.UpdateInactiveEmployees(_date.Now.AddDays(-7), cancellationToken);
            return Unit.Value;
        }
    }
}
