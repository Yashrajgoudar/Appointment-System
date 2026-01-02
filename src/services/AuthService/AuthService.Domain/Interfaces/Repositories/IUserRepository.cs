using AuthService.Domain.Entities;

namespace AuthService.Domain.Interfaces.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByEmailWithRolesAsync(string email);
        Task<IEnumerable<User>> GetByTenantIdAsync(Guid tenantId);
        Task<bool> EmailExistsAsync(string email);
        Task<User?> GetWithRefreshTokensAsync(Guid userId);
    }
}
