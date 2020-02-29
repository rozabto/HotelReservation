using System;
using Application.Common.Models;
using Domain.Enums;
using MediatR;

namespace Application.Search.Queries.SearchHotelRooms
{
    public class SearchHotelRoomsQuery : IRequest<SearchHotelRoomsResponse>
    {
        public string Term { get; set; }
        public DateTime? AvailableFrom { get; set; }
        public DateTime? AvailableTo { get; set; }
        public int? Capacity { get; set; }
        public int Page { get; set; }
        public RoomType? RoomType { get; set; }
        public decimal? Start { get; set; }
        public decimal? End { get; set; }
        public SortBy SortBy { get; set; }
    }
}