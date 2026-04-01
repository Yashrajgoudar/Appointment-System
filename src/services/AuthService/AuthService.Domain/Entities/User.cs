using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthService.Domain.Entities
{
    public class User
    {
        public Guid UserId { get; set; }
        public Guid TenantId { get; set; }
        public string Email { get; set; } = default!;
        public byte[] PasswordHash { get; set; } = default!;
        public byte[] PasswordSalt { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public bool IsEmailConfirmed { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public Tenant Tenant { get; set; } = default!;

        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
