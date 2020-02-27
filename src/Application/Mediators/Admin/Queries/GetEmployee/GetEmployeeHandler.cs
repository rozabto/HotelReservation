using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Repositories;
using MediatR;

namespace Application.Admin.Queries.GetEmployee
{
    public class GetEmployeeHandler : IRequestHandler<GetEmployeeQuery, GetEmployeeResponse>
    {
        private readonly IEmployeeRepository _employee;

        public GetEmployeeHandler(IEmployeeRepository employee)
        {
            _employee = employee ?? throw new ArgumentNullException(nameof(employee));
        }

        public async Task<GetEmployeeResponse> Handle(GetEmployeeQuery request, CancellationToken cancellationToken) =>
            new GetEmployeeResponse
            {
                Employee = await _employee.GetVmById(request.Id, cancellationToken)
            };
    }
}
