using System;
using Domain.Common;

namespace Domain.Entities
{
    public class Reservation : AuditableEntity
    {
        public Reservation()
        {
            Id = Guid.NewGuid().ToString("N");
        }

        public string Id { get; set; }
        public string ReservedRoomId { get; set; }
        public HotelRoom ReservedRoom { get; set; }
        public DateTime ReservedForDate { get; set; }
        public DateTime ReservedUntilDate { get; set; }
        public bool IncludeFood { get; set; }
        public bool AllInclusive { get; set; }
        public bool HasCompleted { get; set; }
        public decimal Price { get; set; }
    }
}
