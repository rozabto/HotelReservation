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
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }

        public void Mapping(Profile profile)
        {
            decimal conversionRate = 1;
            profile.CreateMap<HotelRoom, HotelRoomShortVm>()
                .ForMember(f => f.Image, f => f.MapFrom(s => s.RoomImages.Select(w => w.Image).FirstOrDefault()))
                .ForMember(f => f.Price, f => f.MapFrom(s => (s.PriceForAdults ?? s.RoomPrice).Value * conversionRate));
        }
    }

    public class HotelRoomVm : IMapFrom<HotelRoom>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public RoomType RoomType { get; set; }
        public bool IsEmpty { get; set; }
        public decimal? PriceForAdults { get; set; }
        public decimal? PriceForChildren { get; set; }
        public decimal? RoomPrice { get; set; }
        public int RoomNumber { get; set; }
        public IReadOnlyList<string> Images { get; set; }

        public void Mapping(Profile profile)
        {
            decimal conversionRate = 1;
            profile.CreateMap<HotelRoom, HotelRoomVm>()
                .ForMember(f => f.Images, f => f.MapFrom(s => s.RoomImages.Select(w => w.Image).ToArray()))
                .ForMember(f => f.PriceForAdults, f => f.MapFrom(s => s.PriceForAdults.HasValue ? s.PriceForAdults * conversionRate : null))
                .ForMember(f => f.PriceForChildren, f => f.MapFrom(s => s.PriceForChildren.HasValue ? s.PriceForChildren * conversionRate : null))
                .ForMember(f => f.RoomPrice, f => f.MapFrom(s => s.RoomPrice.HasValue ? s.RoomPrice * conversionRate : null));
        }
    }
}