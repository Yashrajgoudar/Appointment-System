using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Persistence.Repositories
{
    public class TenantRepository : BaseRepository<Tenant>, ITenantRepository
    {
        public TenantRepository(AuthDbContext context) : base(context) { }

        public async Task<Tenant?> GetByDomainAsync(string domain)
        {
            return await _dbSet
                .FirstOrDefaultAsync(t => t.Domain == domain);
        }

        public async Task<IEnumerable<Tenant>> GetActiveTenantsAsync()
        {
            return await _dbSet
                .Where(t => t.IsActive)
                .ToListAsync();
        }
    }
}
