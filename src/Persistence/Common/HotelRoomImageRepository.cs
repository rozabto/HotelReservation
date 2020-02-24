using Application.Common.Repositories;
using Domain.Entities;

namespace Persistence.Common
{
    public class HotelRoomImageRepository : BaseRepository<HotelRoomImage>, IHotelRoomImageRepository
    {
        public HotelRoomImageRepository(IHotelReservationContext context) : base(context)
        {
        }
    }
}