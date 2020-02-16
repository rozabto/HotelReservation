using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Repositories;
using MediatR;

namespace Application.Home.Queries.GetHotelRoom
{
    public class GetHotelRoomHandler : IRequestHandler<GetHotelRoomQuery, GetHotelRoomResponse>
    {
        private readonly IHotelRoomRepository _hotelRoom;

        public GetHotelRoomHandler(IHotelRoomRepository hotelRoom)
        {
            _hotelRoom = hotelRoom ?? throw new ArgumentNullException(nameof(hotelRoom));
        }

        public async Task<GetHotelRoomResponse> Handle(GetHotelRoomQuery request, CancellationToken cancellationToken) =>
            new GetHotelRoomResponse
            {
                Room = await _hotelRoom.GetVmById(request.Id, cancellationToken)
            };
    }
}
