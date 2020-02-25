using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Models;
using Domain.Entities;
using Domain.Enums;
using Domain.Models;

namespace Application.Common.Repositories
{
    public interface IHotelRoomRepository : IRepository<HotelRoom>
    {
        Task<List<HotelRoomShortVm>> GetUnreservedRooms(string userId, int page, int pageCount, CancellationToken token);

        Task<HotelRoomVm> GetVmById(string id, CancellationToken token);

        Task<List<HotelRoomShortVm>> SearchHotelRooms(string term, DateTime? from, DateTime? to, int? capacity, int page, int pageCount, RoomType? type, SortBy sort, CancellationToken token);

        Task<int> SearchedHotelRoomsCount(string term, DateTime? from, DateTime? to, int? capacity, RoomType? type, CancellationToken token);
    }
}