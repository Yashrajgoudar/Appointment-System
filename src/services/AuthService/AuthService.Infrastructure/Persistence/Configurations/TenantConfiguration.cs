using AuthService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthService.Infrastructure.Persistence.Configurations
{
    public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
    {
        public void Configure(EntityTypeBuilder<Tenant> builder)
        {
            //Table Name
            builder.ToTable("Tenants");

            //Primary Key
            builder.HasKey(t=>t.TenantId);

            //Properties
            builder.Property(t => t.TenantId)
                .ValueGeneratedOnAdd(); //Auto Generated GUID

            builder.Property(t => t.TenantName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(t => t.Domain)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(t => t.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(t => t.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            //Indexes
            builder.HasIndex(t => t.IsActive);

            //Relationships
            builder.HasMany(t=>t.Users)
                .WithOne(t => t.Tenant)
                .HasForeignKey(u=>u.TenantId)
                .OnDelete(DeleteBehavior.Cascade); // Delete user when tenant is deleted
        }
    }
}
