using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Enums;

namespace Domain.Models
{
    public class HotelRoomShortVm : IMapFrom<HotelRoom>
    {
        public string Id { get; set; }
        public decimal PriceForAdults { get; set; }
        public string Image { get; set; }

        public void Mapping(Profile profile) =>
            profile.CreateMap<HotelRoom, HotelRoomShortVm>()
                .ForMember(f => f.Image, f => f.MapFrom(s => s.RoomImages.Select(w => w.Image).FirstOrDefault()));
    }

    public class HotelRoomVm : IMapFrom<HotelRoom>
    {
        public string Id { get; set; }
        public int Capacity { get; set; }
        public RoomType RoomType { get; set; }
        public bool IsEmpty { get; set; }
        public decimal PriceForAdults { get; set; }
        public decimal PriceForChildren { get; set; }
        public int RoomNumber { get; set; }
        public IReadOnlyList<string> Images { get; set; }

        public void Mapping(Profile profile) =>
            profile.CreateMap<HotelRoom, HotelRoomVm>()
                .ForMember(f => f.Images, f => f.MapFrom(s => s.RoomImages.Select(w => w.Image).ToArray()));
    }
}
