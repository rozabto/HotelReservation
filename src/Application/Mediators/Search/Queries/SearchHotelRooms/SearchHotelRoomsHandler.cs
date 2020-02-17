using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using Common;
using MediatR;

namespace Application.Search.Queries.SearchHotelRooms
{
    public class SearchHotelRoomsHandler : IRequestHandler<SearchHotelRoomsQuery, SearchHotelRoomsResponse>
    {
        private readonly ICountryService _country;
        private readonly ICurrentUserService _currentUser;
        private readonly IHotelRoomRepository _hotelRoom;

        public SearchHotelRoomsHandler(ICountryService country, ICurrentUserService currentUser, IHotelRoomRepository hotelRoom)
        {
            _country = country ?? throw new ArgumentNullException(nameof(country));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _hotelRoom = hotelRoom ?? throw new ArgumentNullException(nameof(hotelRoom));
        }

        public async Task<SearchHotelRoomsResponse> Handle(SearchHotelRoomsQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Term))
                request.Term = await _country.GetCountry(_currentUser.Ip);

            return new SearchHotelRoomsResponse
            {
                HotelRooms = await _hotelRoom.SearchHotelRooms(
                    request.Term,
                    request.AvailableFrom,
                    request.AvailableTo,
                    request.Capacity,
                    request.Page,
                    20,
                    cancellationToken
                )
            };
        }
    }
}
