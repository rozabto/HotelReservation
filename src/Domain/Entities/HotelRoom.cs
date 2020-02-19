using System;
using System.Collections.Generic;
using Domain.Common;
using Domain.Enums;

namespace Domain.Entities
{
    public class HotelRoom : AuditableEntity
    {
        public HotelRoom()
        {
            RoomImages = new HashSet<HotelRoomImage>();
            Reservations = new HashSet<Reservation>();
            Id = Guid.NewGuid().ToString("N");
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public RoomType RoomType { get; set; }
        public bool IsEmpty { get; set; }
        public decimal? PriceForAdults { get; set; }
        public decimal? PriceForChildren { get; set; }
        public decimal? RoomPrice { get; set; }
        public decimal FoodPrice { get; set; }
        public string Country { get; set; }
        public int RoomNumber { get; set; }

        public ICollection<HotelRoomImage> RoomImages { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
    }
}
