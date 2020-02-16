using Application.Common.Interfaces;
using Application.Common.Repositories;
using Domain.Entities;

namespace Persistence.Common
{
    public class ReservationRepository : BaseRepository<Reservation>, IReservationRepository
    {
        public ReservationRepository(IHotelReservationContext context) : base(context)
        {
        }
    }
}
