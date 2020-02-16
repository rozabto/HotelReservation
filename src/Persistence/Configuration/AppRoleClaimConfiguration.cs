using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration
{
    public class AppRoleClaimConfiguration : IEntityTypeConfiguration<AppRoleClaim>
    {
        public void Configure(EntityTypeBuilder<AppRoleClaim> builder)
        {
            builder.Property(f => f.ClaimType)
                .IsUnicode(false)
                .HasMaxLength(50);

            builder.Property(f => f.ClaimValue)
                .IsUnicode(false)
                .HasMaxLength(50);

            builder.Property(f => f.RoleId)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(32)
                .IsUnicode(false);
        }
    }
}
