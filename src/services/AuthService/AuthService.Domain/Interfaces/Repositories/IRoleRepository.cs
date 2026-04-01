using AuthService.Domain.Entities;

namespace AuthService.Domain.Interfaces.Repositories
{
    public interface IRoleRepository : IBaseRepository<Role>
    {
        Task<Role?> GetByNameAsync(string roleName);
        Task<IEnumerable<Role>> GetSystemRolesAsync();
    }
}
