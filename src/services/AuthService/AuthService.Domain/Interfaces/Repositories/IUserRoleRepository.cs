using AuthService.Domain.Entities;

namespace AuthService.Domain.Interfaces.Repositories
{
    public interface IUserRoleRepository : IBaseRepository<UserRole>
    {
        Task<IEnumerable<UserRole>> GetUserRolesAsync(Guid userId);
        Task<IEnumerable<Role>> GetRolesByUserIdAsync(Guid userId);
        Task AssignRoleToUserAsync(Guid userId, Guid roleId);
        Task RemoveRoleFromUserAsync(Guid userId, Guid roleId);
        Task<bool> UserHasRoleAsync(Guid userId, string roleName);
    }
}
