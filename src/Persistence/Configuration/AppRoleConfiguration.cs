using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration
{
    public class AppRoleConfiguration : IEntityTypeConfiguration<AppRole>
    {
        public void Configure(EntityTypeBuilder<AppRole> builder)
        {
            builder.Property(f => f.ConcurrencyStamp)
                .HasMaxLength(256)
                .IsRequired()
                .IsUnicode(false);

            builder.Property(f => f.Id)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(32)
                .IsUnicode(false);

            builder.Property(f => f.Name)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(f => f.NormalizedName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
        }
    }
}