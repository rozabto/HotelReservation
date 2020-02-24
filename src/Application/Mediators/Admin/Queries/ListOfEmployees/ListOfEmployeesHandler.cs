using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Repositories;
using MediatR;

namespace Application.Admin.Queries.ListOfEmployees
{
    public class ListOfEmployeesHandler : IRequestHandler<ListOfEmployeesQuery, ListOfEmployeesResponse>
    {
        private readonly IEmployeeRepository _employee;

        public ListOfEmployeesHandler(IEmployeeRepository employee)
        {
            _employee = employee ?? throw new ArgumentNullException(nameof(employee));
        }

        public async Task<ListOfEmployeesResponse> Handle(ListOfEmployeesQuery request, CancellationToken cancellationToken) =>
            new ListOfEmployeesResponse
            {
                Employees = await _employee.GetEmployees(request.Page, 50, cancellationToken)
            };
    }
}