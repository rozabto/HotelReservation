using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Repositories;
using MediatR;

namespace Application.Search.Queries.SearchHotelRooms
{
    public class SearchHotelRoomsHandler : IRequestHandler<SearchHotelRoomsQuery, SearchHotelRoomsResponse>
    {
        private readonly IHotelRoomRepository _hotelRoom;

        public SearchHotelRoomsHandler(IHotelRoomRepository hotelRoom)
        {
            _hotelRoom = hotelRoom ?? throw new ArgumentNullException(nameof(hotelRoom));
        }

        public async Task<SearchHotelRoomsResponse> Handle(SearchHotelRoomsQuery request, CancellationToken cancellationToken) =>
            new SearchHotelRoomsResponse
            {
                HotelRooms = await _hotelRoom.SearchHotelRooms(
                        request.Term,
                        request.AvailableFrom,
                        request.AvailableTo,
                        request.Capacity,
                        request.Page,
                        20,
                        request.RoomType,
                        request.SortBy,
                        cancellationToken
                    ),
                Count = await _hotelRoom.SearchedHotelRoomsCount(
                        request.Term,
                        request.AvailableFrom,
                        request.AvailableTo,
                        request.Capacity,
                        request.RoomType,
                        cancellationToken
                    )
            };
    }
}