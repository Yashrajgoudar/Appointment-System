using System.ComponentModel.DataAnnotations;

namespace AuthService.Domain.Entities
{
    public class Tenant
    {
        public Guid TenantId { get; set; }
        public string TenantName { get; set; } = default!;
        public string Domain {  get; set; } = default!;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
