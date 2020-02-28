using Domain.Models;

namespace Application.Home.Queries.GetHotelRoom
{
    public class GetHotelRoomResponse
    {
        public HotelRoomVm Room { get; set; }
        public string CurrencyCode { get; set; }
    }
}