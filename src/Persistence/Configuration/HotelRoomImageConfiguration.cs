using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration
{
    public class HotelRoomImageConfiguration : IEntityTypeConfiguration<HotelRoomImage>
    {
        public void Configure(EntityTypeBuilder<HotelRoomImage> builder)
        {
            builder.Property(f => f.Id)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(32)
                .IsUnicode(false);

            builder.Property(f => f.HotelRoomId)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(32)
                .IsUnicode(false);

            builder.Property(f => f.Image)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.HasOne(f => f.HotelRoom)
                .WithMany(f => f.RoomImages)
                .HasForeignKey(f => f.HotelRoomId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}