using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Repositories;
using Common;
using MediatR;

namespace Application.Home.Queries.GetHomePage
{
    public class GetHomePageHandler : IRequestHandler<GetHomePageQuery, GetGomePageResponse>
    {
        private readonly IHotelRoomRepository _hotelRoom;
        private readonly ICurrentUserService _currentUser;

        public GetHomePageHandler(IHotelRoomRepository hotelRoom, ICurrentUserService currentUser)
        {
            _hotelRoom = hotelRoom ?? throw new ArgumentNullException(nameof(hotelRoom));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }

        public async Task<GetGomePageResponse> Handle(GetHomePageQuery request, CancellationToken cancellationToken) =>
            new GetGomePageResponse
            {
                HotelRooms = await _hotelRoom.GetUnreservedRooms(_currentUser.IsAuthenticated ? _currentUser.User.Id : null, request.Page, 20, cancellationToken)
            };
    }
}
