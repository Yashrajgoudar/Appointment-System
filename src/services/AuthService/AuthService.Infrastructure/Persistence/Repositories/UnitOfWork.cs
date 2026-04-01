using AuthService.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace AuthService.Infrastructure.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AuthDbContext _context;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(
            AuthDbContext context,
            IUserRepository users,
            IRoleRepository roles,
            ITenantRepository tenants,
            IUserRoleRepository userRoles,
            IRefreshTokenRepository refreshTokens)
        {
            _context = context;
            Users = users;
            Roles = roles;
            Tenants = tenants;
            UserRoles = userRoles;
            RefreshTokens = refreshTokens;
        }

        public IUserRepository Users { get; }
        public IRoleRepository Roles { get; }
        public ITenantRepository Tenants { get; }
        public IUserRoleRepository UserRoles { get; }
        public IRefreshTokenRepository RefreshTokens { get; }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}
