using System.Collections.Generic;
using Domain.Models;

namespace Application.Home.Queries.GetHomePage
{
    public class GetGomePageResponse
    {
        public IReadOnlyList<HotelRoomShortVm> HotelRooms { get; set; }
    }
}
