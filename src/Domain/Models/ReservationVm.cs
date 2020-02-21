using System;
using AutoMapper;
using Domain.Common;
using Domain.Entities;

namespace Domain.Models
{
    public class ReservationVm : IMapFrom<Reservation>
    {
        public string Id { get; set; }
        public HotelRoomShortVm ReservedRoom { get; set; }
        public DateTime ReservedForDate { get; set; }
        public DateTime ReservedUntilDate { get; set; }
        public bool IncludeFood { get; set; }
        public bool AllInclusive { get; set; }
        public decimal Price { get; set; }

        public void Mapping(Profile profile) =>
            profile.CreateMap<Reservation, ReservationVm>()
                .ForMember(f => f.ReservedRoom, f => f.MapFrom(s => s.ReservedRoom));
    }
}
