using AuthService.Application.Common.Interfaces;
using AuthService.Domain.Interfaces.Repositories;
using AuthService.Infrastructure.Identity;
using AuthService.Infrastructure.Persistence;
using AuthService.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // DbContext
            services.AddDbContext<AuthDbContext>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(AuthDbContext).Assembly.FullName)));

            // Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<ITenantRepository, TenantRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            // Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
    }
}
