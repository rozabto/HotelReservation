using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration
{
    public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.Property(f => f.CreatedById)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(32)
                .IsUnicode(false);

            builder.Property(f => f.EditedById)
                .IsFixedLength()
                .HasMaxLength(32)
                .IsUnicode(false);

            builder.Property(f => f.Id)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(32)
                .IsUnicode(false);

            builder.Property(f => f.ReservedRoomId)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(32)
                .IsUnicode(false);

            builder.Property(f => f.AuthCode)
                .HasMaxLength(35)
                .IsUnicode(false);

            builder.HasOne(f => f.CreatedBy)
                .WithMany(f => f.Reservations)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(f => f.EditedBy)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}