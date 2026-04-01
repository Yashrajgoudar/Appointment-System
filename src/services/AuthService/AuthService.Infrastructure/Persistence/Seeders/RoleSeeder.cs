using AuthService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Persistence.Seeders
{
    public static class RoleSeeder
    {
        public static async Task SeedRolesAsync(AuthDbContext context)
        {
            if(await context.Roles.AnyAsync())
            {
                return;
            }

            var roles = new List<Role>
            {
                new Role
                {
                    RoleId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    RoleName = "SuperAdmin",
                    Description = "Platform administrator with full access"
                },
                new Role
                {
                    RoleId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    RoleName = "TenantAdmin",
                    Description = "Tenant administrator with full tenant access"
                },
                new Role
                {
                    RoleId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    RoleName = "Staff",
                    Description = "Staff member who provides services"
                },
                new Role
                {
                    RoleId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    RoleName = "Customer",
                    Description = "Customer who books appointments"
                }
            };

            await context.Roles.AddRangeAsync(roles);
            await context.SaveChangesAsync();
        }
    }
}
