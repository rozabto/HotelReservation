using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration
{
    public class AppUserLoginConfiguration : IEntityTypeConfiguration<AppUserLogin>
    {
        public void Configure(EntityTypeBuilder<AppUserLogin> builder)
        {
            builder.Property(f => f.LoginProvider)
                .IsRequired()
                .HasMaxLength(128);

            builder.Property(f => f.ProviderKey)
                .IsRequired()
                .HasMaxLength(128);

            builder.Property(f => f.UserId)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(32)
                .IsUnicode(false);

            builder.Property(f => f.ProviderDisplayName)
                .HasMaxLength(100);

            builder.HasKey(f => new { f.LoginProvider, f.ProviderKey });
        }
    }
}