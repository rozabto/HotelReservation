using Common;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Production
{
    public class ProductionDbContext : HotelReservationContext
    {
        public ProductionDbContext(DbContextOptions options, IDateTime date, ICurrentUserService currentUser) : base(options, date, currentUser)
        {
        }
    }
}
