using AuthService.Domain.Entities;

namespace AuthService.Domain.Interfaces.Repositories
{
    public interface IRefreshTokenRepository : IBaseRepository<RefreshToken>
    {
        Task<RefreshToken?> GetByTokenHashAsync(string tokenHash);
        Task<IEnumerable<RefreshToken>> GetActiveTokensByUserIdAsync(Guid userId);
        Task RevokeTokenAsync(Guid tokenId);
        Task RevokeAllUserTokensAsync(Guid userId);
        Task<int> DeleteExpiredTokensAsync();
        Task<bool> IsTokenValidAsync(string tokenHash);
    }
}
