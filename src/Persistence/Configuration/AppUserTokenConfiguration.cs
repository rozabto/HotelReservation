using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration
{
    public class AppUserTokenConfiguration : IEntityTypeConfiguration<AppUserToken>
    {
        public void Configure(EntityTypeBuilder<AppUserToken> builder)
        {
            builder.Property(f => f.LoginProvider)
                .IsRequired()
                .HasMaxLength(128);

            builder.Property(f => f.Name)
                .IsRequired()
                .HasMaxLength(128);

            builder.Property(f => f.Value)
                .HasMaxLength(100);

            builder.Property(f => f.UserId)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(32)
                .IsUnicode(false);
        }
    }
}
