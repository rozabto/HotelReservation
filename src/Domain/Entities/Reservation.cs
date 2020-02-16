using System;
using System.Collections.Generic;
using Domain.Common;

namespace Domain.Entities
{
    public class Reservation : AuditableEntity
    {
        public Reservation()
        {
            UsersReservations = new HashSet<UserReservation>();
            Id = Guid.NewGuid().ToString("N");
        }

        public string Id { get; set; }
        public string ReservedRoomId { get; set; }
        public HotelRoom ReservedRoom { get; set; }
        public DateTime ReservedForDate { get; set; }
        public DateTime ReservedUntilDate { get; set; }
        public bool IncludeFood { get; set; }
        public bool AllInclusive { get; set; }
        public decimal Price { get; set; }

        public ICollection<UserReservation> UsersReservations { get; set; }
    }
}
