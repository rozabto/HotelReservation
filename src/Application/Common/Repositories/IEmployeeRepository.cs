using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Models;

namespace Application.Common.Repositories
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<List<EmployeeShortVm>> GetEmployees(int page, int pageCount, DateTime afterDate, CancellationToken token);

        Task<EmployeeVm> GetVmById(string id, CancellationToken token);

        Task<Employee> GetEmployeeWithUser(string id, CancellationToken token);

        Task<Employee> GetEmployeeByUser(string id, CancellationToken token);

        Task UpdateInactiveEmployees(DateTime date, CancellationToken token);
    }
}