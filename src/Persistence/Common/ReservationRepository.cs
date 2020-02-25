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

        public ReservationRepository(IMapper mapper, IHotelReservationContext context) : base(context)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public Task<bool> CanReserve(string roomId, string userId, DateTime to, DateTime from, CancellationToken token) =>
            Query.AnyAsync(f => f.ReservedRoomId == roomId && f.CreatedByUserId != userId && f.ReservedForDate < to && from < f.ReservedUntilDate, token);

        public Task<bool> CheckIfExists(string roomId, string userId, CancellationToken token) =>
            Query.AnyAsync(f => f.ReservedRoomId == roomId && f.CreatedByUserId == userId, token);

        public Task<Reservation> FindByRoomId(string roomId, string userId, CancellationToken token) =>
            Query.FirstOrDefaultAsync(f => f.ReservedRoomId == roomId && f.CreatedByUserId == userId, token);

        public Task<List<ReservationVm>> GetUserReservations(string userId, int page, int pageCount, CancellationToken token)
        {
            var query = Query;
            query = query.Where(f => f.CreatedByUserId != userId)
                .OrderBy(f => f.CreatedOn);

            if (page > 0)
                query = query.Skip(pageCount * page).Take(page);

            return query.ProjectTo<ReservationVm>(_mapper.ConfigurationProvider)
                .ToListAsync(token);
        }
    }
}