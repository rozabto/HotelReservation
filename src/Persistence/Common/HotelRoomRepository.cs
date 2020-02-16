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
    public class HotelRoomRepository : BaseRepository<HotelRoom>, IHotelRoomRepository
    {
        private readonly IMapper _mapper;

        public HotelRoomRepository(IMapper mapper, IHotelReservationContext context) : base(context)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public Task<List<HotelRoomShortVm>> GetUnreservedRooms(string userId, int page, int pageCount, CancellationToken token)
        {
            if (userId != null)
            {
                Query.Include(f => f.Reservations)
                    .ThenInclude(f => f.UsersReservations)
                    .Where(f => f.Reservations.Any(s => s.UsersReservations.Any(w => w.UserId != userId)));
            }

            Query.OrderBy(f => f.CreatedOn);

            if (page > 0)
                Query.Skip(pageCount * page).Take(page);

            return Query.ProjectTo<HotelRoomShortVm>(_mapper.ConfigurationProvider)
                .ToListAsync(token);
        }

        public Task<HotelRoomVm> GetVmById(string id, CancellationToken token) =>
            Query.Where(f => f.Id == id)
                .ProjectTo<HotelRoomVm>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(token);
    }
}
