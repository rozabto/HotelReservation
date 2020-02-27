using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Repositories;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Common
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        private readonly IMapper _mapper;

        public EmployeeRepository(IMapper mapper, IHotelReservationContext context) : base(context)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public Task<List<EmployeeShortVm>> GetEmployees(int page, int pageCount, DateTime afterDate, CancellationToken token) =>
            Query.Skip(pageCount * page)
                .Take(pageCount)
                .ProjectTo<EmployeeShortVm>(_mapper.ConfigurationProvider, new { afterDate })
                .ToListAsync(token);

        public Task<EmployeeVm> GetVmById(string id, CancellationToken token) =>
            Query.Where(f => f.Id == id)
                .ProjectTo<EmployeeVm>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(token);

        public Task<Employee> GetEmployeeWithUser(string id, CancellationToken token) =>
            Query.Include(f => f.User)
                .FirstOrDefaultAsync(f => f.Id == id, token);

        public Task<Employee> GetEmployeeByUser(string id, CancellationToken token) =>
            Query.FirstOrDefaultAsync(f => f.UserId == id, token);

        public Task UpdateInactiveEmployees(DateTime date, CancellationToken token) =>
            Query.Include(f => f.User)
                .ThenInclude(f => f.Reservations)
                .Where(f => f.IsActive && f.User.Reservations.OrderByDescending(w => w.CreatedOn).Select(w => w.CreatedOn).FirstOrDefault() < date)
                .UpdateFromQueryAsync(_ => new Employee { IsActive = false }, token);
    }
}