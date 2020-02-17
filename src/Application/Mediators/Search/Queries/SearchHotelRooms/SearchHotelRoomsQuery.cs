using System;
using MediatR;

namespace Application.Search.Queries.SearchHotelRooms
{
    public class SearchHotelRoomsQuery : IRequest<SearchHotelRoomsResponse>
    {
        public string Term { get; set; }
        public DateTime AvailableFrom { get; set; }
        public DateTime AvailableTo { get; set; }
        public int Capacity { get; set; }
        public int Page { get; set; }
    }
}
