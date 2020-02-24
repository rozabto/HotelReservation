using Common;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Development
{
    public class DevelopmentDbContext : HotelReservationContext
    {
        public DevelopmentDbContext(DbContextOptions options, IDateTime date, ICurrentUserService currentUser) : base(options, date, currentUser)
        {
        }
    }
}