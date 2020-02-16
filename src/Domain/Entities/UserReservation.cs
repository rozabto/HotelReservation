using Domain.Common;

namespace Domain.Entities
{
    public class UserReservation : AuditableEntity
    {
        public string ReservationId { get; set; }
        public Reservation Reservation { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
    }
}
