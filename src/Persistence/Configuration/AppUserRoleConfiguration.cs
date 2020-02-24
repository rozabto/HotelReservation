using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration
{
    public class AppUserRoleConfiguration : IEntityTypeConfiguration<AppUserRole>
    {
        public void Configure(EntityTypeBuilder<AppUserRole> builder)
        {
            builder.Property(f => f.RoleId)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(32)
                .IsUnicode(false);

            builder.Property(f => f.UserId)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(32)
                .IsUnicode(false);

            builder.HasKey(f => new { f.UserId, f.RoleId });

            builder.HasOne(f => f.Role)
                .WithMany(f => f.UsersRoles)
                .HasForeignKey(f => f.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(f => f.User)
                .WithMany(f => f.UsersRoles)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}