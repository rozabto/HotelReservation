using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration
{
    public class HotelRoomConfiguration : IEntityTypeConfiguration<HotelRoom>
    {
        public void Configure(EntityTypeBuilder<HotelRoom> builder)
        {
            builder.Property(f => f.Id)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(32)
                .IsUnicode(false);

            builder.Property(f => f.CreatedById)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(32)
                .IsUnicode(false);

            builder.Property(f => f.EditedById)
                .IsFixedLength()
                .HasMaxLength(32)
                .IsUnicode(false);

            builder.Property(f => f.Name)
                .HasMaxLength(50)
                .IsRequired()
                .IsUnicode(false);

            builder.HasOne(f => f.CreatedBy)
                .WithMany(f => f.CreatedRooms)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(f => f.EditedBy)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}