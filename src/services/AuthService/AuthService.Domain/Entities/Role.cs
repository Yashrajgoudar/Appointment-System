using System.ComponentModel.DataAnnotations;

namespace AuthService.Domain.Entities
{
    public class Role
    {
        public Guid RoleId { get; set; }
        public string RoleName { get; set; } = default!;
        public string Description { get; set; } = default!;

    }
}
