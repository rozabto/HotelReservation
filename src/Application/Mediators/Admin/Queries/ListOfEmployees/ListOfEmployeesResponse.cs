using System.Collections.Generic;
using Domain.Models;

namespace Application.Admin.Queries.ListOfEmployees
{
    public class ListOfEmployeesResponse
    {
        public IReadOnlyList<EmployeeVm> Employees { get; set; }
    }
}
