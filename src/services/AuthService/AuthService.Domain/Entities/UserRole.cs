using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthService.Domain.Entities
{
    public class UserRole
    {
        public Guid UserRoleId { get; set; }
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public DateTime AssignedAt { get; set; }
        public User User { get; set; } = default!;
        public Role Role { get; set; } = default!;
    }
}
