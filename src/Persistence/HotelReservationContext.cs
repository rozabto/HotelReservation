using System.Threading;
using System.Threading.Tasks;
using Common;
using Domain.Common;
using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class HotelReservationContext : IdentityDbContext<AppUser, AppRole, string, AppUserClaim, AppUserRole, AppUserLogin, AppRoleClaim, AppUserToken>, IHotelReservationContext
    {
        private readonly IDateTime _date;
        private readonly ICurrentUserService _currentUser;

        public HotelReservationContext(DbContextOptions options, IDateTime date, ICurrentUserService currentUser) : base(options)
        {
            _date = date;
            _currentUser = currentUser;
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<HotelRoom> HotelRooms { get; set; }
        public DbSet<HotelRoomImage> HotelRoomImages { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach(var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedById = _currentUser.User.Id;
                        entry.Entity.CreatedOn = _date.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.EditedById = _currentUser.User.Id;
                        entry.Entity.EditedOn = _date.Now;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(HotelReservationContext).Assembly);
        }
    }
}