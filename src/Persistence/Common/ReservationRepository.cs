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
    public class ReservationRepository : BaseRepository<Reservation>, IReservationRepository
    {
        private readonly IMapper _mapper;
        private readonly bool isEnvProd;

        public ReservationRepository(IMapper mapper, IHotelReservationContext context) : base(context)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            isEnvProd = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production";
        }

        public Task<bool> CanReserve(string roomId, DateTime to, DateTime from, CancellationToken token) =>
            Query.AnyAsync(f => f.ReservedRoomId == roomId && f.ReservedForDate < to && from < f.ReservedUntilDate && f.DeletedOn == null, token);

        public Task<bool> CheckIfExists(string roomId, string userId, CancellationToken token) =>
            Query.AnyAsync(f => f.ReservedRoomId == roomId && f.CreatedById == userId && !f.TransactionId.HasValue && f.DeletedOn == null, token);

        public Task DeleteExpired(DateTime date, CancellationToken token) =>
            isEnvProd ? Query.Where(f => f.TransactionId == null && date - f.CreatedOn > TimeSpan.FromHours(1))
                    .DeleteFromQueryAsync(token)
                : Query.Where(f => f.TransactionId == null && EF.Functions.DateDiffHour(date, f.CreatedOn) > 1)
                    .DeleteFromQueryAsync(token);

        public Task<Reservation> FindByRoomId(string roomId, string userId, CancellationToken token) =>
            Query.FirstOrDefaultAsync(f => f.ReservedRoomId == roomId && f.CreatedById == userId, token);

        public Task<List<ReservationVm>> GetUserReservations(string userId, int page, int pageCount, CancellationToken token)
        {
            var query = Query;
            query = query.Where(f => f.CreatedById == userId && f.DeletedOn == null)
                .OrderBy(f => f.CreatedOn);

            if (page > 0)
                query = query.Skip(pageCount * page);

            query = query.Take(pageCount);

            return query.ProjectTo<ReservationVm>(_mapper.ConfigurationProvider)
                .ToListAsync(token);
        }
    }
}