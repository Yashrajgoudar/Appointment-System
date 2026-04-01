using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Persistence.Repositories
{
    public class RefreshTokenRepository : BaseRepository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(AuthDbContext context) : base(context) { }

        public async Task<RefreshToken?> GetByTokenHashAsync(string tokenHash)
        {
            return await _dbSet
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.TokenHash == tokenHash);
        }

        public async Task<IEnumerable<RefreshToken>> GetActiveTokensByUserIdAsync(Guid userId)
        {
            return await _dbSet
                .Where(rt => rt.UserId == userId &&
                            !rt.IsRevoked &&
                            rt.ExpiresAt > DateTime.UtcNow)
                .ToListAsync();
        }

        public async Task RevokeTokenAsync(Guid tokenId)
        {
            var token = await _dbSet.FindAsync(tokenId);

            if (token != null)
            {
                token.IsRevoked = true;
                token.CreatedAt = DateTime.UtcNow;
                _dbSet.Update(token);
            }
        }

        public async Task RevokeAllUserTokensAsync(Guid userId)
        {
            var tokens = await _dbSet
                .Where(rt => rt.UserId == userId && !rt.IsRevoked)
                .ToListAsync();

            foreach (var token in tokens)
            {
                token.IsRevoked = true;
                token.CreatedAt = DateTime.UtcNow;
            }

            _dbSet.UpdateRange(tokens);
        }

        public async Task<int> DeleteExpiredTokensAsync()
        {
            var expiredTokens = await _dbSet
                .Where(rt => rt.ExpiresAt < DateTime.UtcNow || rt.IsRevoked)
                .ToListAsync();

            _dbSet.RemoveRange(expiredTokens);

            return expiredTokens.Count;
        }

        public async Task<bool> IsTokenValidAsync(string tokenHash)
        {
            return await _dbSet
                .AnyAsync(rt => rt.TokenHash == tokenHash &&
                               !rt.IsRevoked &&
                               rt.ExpiresAt > DateTime.UtcNow);
        }
    }
}
