using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Persistence.Repositories
{
    public class UserRoleRepository : BaseRepository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(AuthDbContext context) : base(context) { }

        public async Task<IEnumerable<UserRole>> GetUserRolesAsync(Guid userId)
        {
            return await _dbSet
                .Include(ur => ur.Role)
                .Where(ur => ur.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Role>> GetRolesByUserIdAsync(Guid userId)
        {
            return await _dbSet
                .Include(ur => ur.Role)
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.Role)
                .ToListAsync();
        }

        public async Task AssignRoleToUserAsync(Guid userId, Guid roleId)
        {
            var userRole = new UserRole
            {
                UserId = userId,
                RoleId = roleId,
                AssignedAt = DateTime.UtcNow
            };

            await _dbSet.AddAsync(userRole);
        }

        public async Task RemoveRoleFromUserAsync(Guid userId, Guid roleId)
        {
            var userRole = await _dbSet
                .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);

            if (userRole != null)
            {
                _dbSet.Remove(userRole);
            }
        }

        public async Task<bool> UserHasRoleAsync(Guid userId, string roleName)
        {
            return await _dbSet
                .Include(ur => ur.Role)
                .AnyAsync(ur => ur.UserId == userId && ur.Role.RoleName == roleName);
        }
    }
}
