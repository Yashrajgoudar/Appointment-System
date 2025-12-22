using AuthService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthService.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            //Table Name
            builder.ToTable("Users");

            //Primary Key
            builder.HasKey(t => t.UserId);

            //Properties
            builder.Property(t => t.UserId)
                .ValueGeneratedOnAdd(); //Auto Generated GUID

            builder.Property(t => t.Email)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(t => t.PasswordHash)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(t => t.PasswordSalt)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(t => t.FirstName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(t => t.LastName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(t => t.PhoneNumber)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(t => t.IsEmailConfirmed)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(t => t.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(t => t.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(t => t.LastLoginAt)
                .IsRequired(false);

            //Indexes
            builder.HasIndex(t => t.Email)
                .IsUnique();

            builder.HasIndex(t => t.TenantId);

            builder.HasIndex(t => t.IsActive);

            //Relationships
            builder.HasOne(u=>u.Tenant)
                .WithMany(t=>t.Users)
                .HasForeignKey(u => u.TenantId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.UserRoles)
                .WithOne(u => u.User)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.RefreshTokens)
                .WithOne(u => u.User)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
