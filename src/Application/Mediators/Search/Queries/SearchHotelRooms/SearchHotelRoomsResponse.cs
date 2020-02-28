using System.Collections.Generic;
using Domain.Models;

namespace Application.Search.Queries.SearchHotelRooms
{
    public class SearchHotelRoomsResponse
    {
        public IReadOnlyList<HotelRoomShortVm> HotelRooms { get; set; }
        public int Count { get; set; }
        public int HighestPrice { get; set; }
        public string CurrencyCode { get; set; }
    }
}