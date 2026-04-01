using AuthService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthService.Infrastructure.Persistence.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles");

            builder.HasKey(r => r.RoleId);

            builder.Property(r => r.RoleId)
                .ValueGeneratedOnAdd();

            builder.Property(r => r.RoleName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(r => r.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.HasIndex(r => r.RoleName)
                .IsUnique();
        }
    }
}