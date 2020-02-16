using Application.Common.Interfaces;
using Application.Common.Repositories;
using Domain.Entities;

namespace Persistence.Common
{
    public class UserReservationRepository : BaseRepository<UserReservation>, IUserReservationRepository
    {
        public UserReservationRepository(IHotelReservationContext context) : base(context)
        {
        }
    }
}
