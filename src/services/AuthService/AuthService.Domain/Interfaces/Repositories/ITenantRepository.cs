using AuthService.Domain.Entities;

namespace AuthService.Domain.Interfaces.Repositories
{
    public interface ITenantRepository : IBaseRepository<Tenant>
    {
        Task<Tenant?> GetByDomainAsync(string domain);
        Task<IEnumerable<Tenant>> GetActiveTenantsAsync();
    }
}
