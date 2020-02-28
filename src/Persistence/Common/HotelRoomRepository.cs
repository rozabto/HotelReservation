using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Models;
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

        public Task<HotelRoom> GetRoomWithReservationsOverDate(string id, DateTime date, CancellationToken token) =>
            Query.Include(f => f.Reservations.Where(f => f.ReservedForDate < date))
                .FirstOrDefaultAsync(f => f.Id == id, token);

        public Task<HotelRoomVm> GetVmById(string id, decimal conversionRate, CancellationToken token) =>
            Query.Where(f => f.Id == id)
                .ProjectTo<HotelRoomVm>(_mapper.ConfigurationProvider, new { conversionRate })
                .FirstOrDefaultAsync(token);

        public Task<decimal> HighestPricesRoomSearch(string term, DateTime? from, DateTime? to, int? capacity, RoomType? type, CancellationToken token)
        {
            var query = Query.Where(f => f.DeletedOn == null);
            query = query.Include(f => f.Reservations);

            query = FilterSearch(query, term, from, to, capacity, type);

            return query.MaxAsync(f => (f.PriceForAdults ?? f.RoomPrice).Value, token);
        }

        public Task<int> SearchedHotelRoomsCount(string term, DateTime? from, DateTime? to, int? capacity, RoomType? type, CancellationToken token)
        {
            var query = Query.Where(f => f.DeletedOn == null);
            query = query.Include(f => f.Reservations);

            query = FilterSearch(query, term, from, to, capacity, type);

            return query.CountAsync(token);
        }

        public Task<List<HotelRoomShortVm>> SearchHotelRooms(string term, decimal conversionRate, DateTime? from, DateTime? to, int? capacity, int page, int pageCount, RoomType? type, SortBy sort, CancellationToken token)
        {
            var query = Query.Where(f => f.DeletedOn == null);
            query = query.Include(f => f.Reservations);

            query = FilterSearch(query, term, from, to, capacity, type);

            switch (sort)
            {
                case SortBy.New:
                    query = query.OrderBy(f => f.CreatedOn);
                    break;

                case SortBy.Popular:
                    query = query.OrderBy(f => f.Reservations.Count);
                    break;

                case SortBy.LowerPrice:
                    query = query.OrderBy(f => f.PriceForAdults ?? f.RoomPrice);
                    break;

                case SortBy.HigherPrice:
                    query = query.OrderByDescending(f => f.PriceForAdults ?? f.RoomPrice);
                    break;
            }

            if (page > 0)
                query = query.Skip(pageCount * page);

            query = query.Take(pageCount);

            return query.ProjectTo<HotelRoomShortVm>(_mapper.ConfigurationProvider, new { conversionRate })
                .ToListAsync(token);
        }

        private IQueryable<HotelRoom> FilterSearch(IQueryable<HotelRoom> query, string term, DateTime? from, DateTime? to, int? capacity, RoomType? type)
        {
            if (from.HasValue && to.HasValue)
                query = query.Where(f => !f.Reservations.Any(f => f.ReservedForDate < to && from < f.ReservedUntilDate));

            if (!string.IsNullOrWhiteSpace(term))
                query = query.Where(f => f.Name == term);

            if (capacity.HasValue)
                query = query.Where(f => f.Capacity >= capacity);

            if (type.HasValue)
                query = query.Where(f => f.RoomType == type);

            return query;
        }
    }
}