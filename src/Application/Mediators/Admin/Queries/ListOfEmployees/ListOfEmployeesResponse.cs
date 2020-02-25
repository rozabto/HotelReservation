using System.Collections.Generic;
using Domain.Models;

namespace Application.Admin.Queries.ListOfEmployees
{
    public class ListOfEmployeesResponse
    {
        public IReadOnlyList<EmployeeShortVm> Employees { get; set; }
    }
}