using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Persistence.Repositories
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(AuthDbContext context) : base(context) { }

        public async Task<Role?> GetByNameAsync(string roleName)
        {
            return await _dbSet
                .FirstOrDefaultAsync(r => r.RoleName == roleName);
        }

        public async Task<IEnumerable<Role>> GetSystemRolesAsync()
        {
            return await _dbSet
                .ToListAsync();
        }

    }
}
