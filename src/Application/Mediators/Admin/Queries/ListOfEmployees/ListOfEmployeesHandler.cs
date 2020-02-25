using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Repositories;
using Common;
using MediatR;

namespace Application.Admin.Queries.ListOfEmployees
{
    public class ListOfEmployeesHandler : IRequestHandler<ListOfEmployeesQuery, ListOfEmployeesResponse>
    {
        private readonly IEmployeeRepository _employee;
        private readonly IDateTime _date;

        public ListOfEmployeesHandler(IEmployeeRepository employee, IDateTime date)
        {
            _employee = employee ?? throw new ArgumentNullException(nameof(employee));
            _date = date ?? throw new ArgumentNullException(nameof(date));
        }

        public async Task<ListOfEmployeesResponse> Handle(ListOfEmployeesQuery request, CancellationToken cancellationToken) =>
            new ListOfEmployeesResponse
            {
                Employees = await _employee.GetEmployees(request.Page, 50, _date.Now.AddDays(-6 * 7), cancellationToken)
            };
    }
}