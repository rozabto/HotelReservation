using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.Property(f => f.Id)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(32)
                .IsUnicode(false);

            builder.Property(f => f.UserName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(f => f.NormalizedUserName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(f => f.Email)
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(f => f.NormalizedEmail)
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(f => f.PasswordHash)
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(f => f.ConcurrencyStamp)
                .IsFixedLength()
                .HasMaxLength(36)
                .IsRequired();

            builder.Property(f => f.PhoneNumber)
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(f => f.SecurityStamp)
                .IsFixedLength()
                .HasMaxLength(32)
                .IsRequired()
                .IsUnicode(false);

            builder.Property(f => f.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(f => f.LastName)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
