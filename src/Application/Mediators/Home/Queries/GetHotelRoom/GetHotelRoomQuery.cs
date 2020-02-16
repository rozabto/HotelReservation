using MediatR;

namespace Application.Home.Queries.GetHotelRoom
{
    public class GetHotelRoomQuery : IRequest<GetHotelRoomResponse>
    {
        public string Id { get; set; }
    }
}
