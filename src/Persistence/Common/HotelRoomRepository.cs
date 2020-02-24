using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Repositories;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Domain.Enums;
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
                    .Where(f => f.Reservations.Any(w => w.CreatedByUserId != userId));
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

        public Task<int> SearchedHotelRoomsCount(string term, DateTime? from, DateTime? to, int? capacity, int page, int pageCount, RoomType? type, CancellationToken token)
        {
            Query.Include(f => f.Reservations)
                .Where(f => !f.Reservations.Any(f => f.ReservedForDate < to && from < f.ReservedUntilDate)
                    && f.Capacity >= capacity)
                .OrderBy(f => f.CreatedOn);

            if (!string.IsNullOrWhiteSpace(term))
                Query.Where(f => f.Name == term);

            if (capacity.HasValue)
                Query.Where(f => f.Capacity >= capacity);

            if (type.HasValue)
                Query.Where(f => f.RoomType == type);

            if (page > 0)
                Query.Skip(pageCount * page).Take(page);

            return Query.CountAsync(token);
        }

        public Task<List<HotelRoomShortVm>> SearchHotelRooms(string term, DateTime? from, DateTime? to, int? capacity, int page, int pageCount, RoomType? type, SortBy sort, CancellationToken token)
        {
            Query.Include(f => f.Reservations);

            if (from.HasValue && to.HasValue)
                Query.Where(f => !f.Reservations.Any(f => f.ReservedForDate < to && from < f.ReservedUntilDate));

            if (!string.IsNullOrWhiteSpace(term))
                Query.Where(f => f.Name == term);

            if (capacity.HasValue)
                Query.Where(f => f.Capacity >= capacity);

            if (type.HasValue)
                Query.Where(f => f.RoomType == type);

            switch (sort)
            {
                case SortBy.New:
                    Query.OrderBy(f => f.CreatedOn);
                    break;

                case SortBy.Popular:
                    Query.OrderBy(f => f.Reservations.Count);
                    break;

                case SortBy.LowerPrice:
                    Query.OrderBy(f => f.PriceForAdults ?? f.RoomPrice);
                    break;

                case SortBy.HigherPrice:
                    Query.OrderByDescending(f => f.PriceForAdults ?? f.RoomPrice);
                    break;
            }

            if (page > 0)
                Query.Skip(pageCount * page).Take(page);

            return Query.ProjectTo<HotelRoomShortVm>(_mapper.ConfigurationProvider)
                .ToListAsync(token);
        }
    }
}