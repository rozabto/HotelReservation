using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration
{
    public class UserReservationConfiguration : IEntityTypeConfiguration<UserReservation>
    {
        public void Configure(EntityTypeBuilder<UserReservation> builder)
        {
            builder.Property(f => f.ReservationId)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(32)
                .IsUnicode(false);

            builder.Property(f => f.UserId)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(32)
                .IsUnicode(false);

            builder.Property(f => f.CreatedByUserId)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(32)
                .IsUnicode(false);

            builder.HasKey(f => new { f.ReservationId, f.UserId });

            builder.HasOne(f => f.Reservation)
                .WithMany(f => f.UsersReservations)
                .HasForeignKey(f => f.ReservationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(f => f.User)
                .WithMany(f => f.UsersReservations)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
