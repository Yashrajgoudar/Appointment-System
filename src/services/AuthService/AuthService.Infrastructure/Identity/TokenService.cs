using AuthService.Application.Common.Interfaces;
using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AuthService.Infrastructure.Identity
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TokenService(
            IConfiguration configuration,
            IRefreshTokenRepository refreshTokenRepository,
            IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
        }

        public string GenerateAccessToken(User user, IEnumerable<string> roles)
        {
            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("FirstName", user.FirstName),
            new Claim("LastName", user.LastName),
            new Claim("TenantId", user.TenantId.ToString())
        };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expirationMinutes = int.Parse(
                _configuration["Jwt:AccessTokenExpirationMinutes"] ?? "15");

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> GenerateRefreshTokenAsync(Guid userId)
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            var tokenHash = Convert.ToBase64String(randomBytes);

            var expirationDays = int.Parse(
                _configuration["Jwt:RefreshTokenExpirationDays"] ?? "7");

            var refreshToken = new RefreshToken
            {
                RefreshTokenId = Guid.NewGuid(),
                UserId = userId,
                TokenHash = tokenHash,
                ExpiresAt = DateTime.UtcNow.AddDays(expirationDays),
                CreatedAt = DateTime.UtcNow,
                IsRevoked = false
            };

            await _refreshTokenRepository.AddAsync(refreshToken);
            await _unitOfWork.SaveChangesAsync();

            return tokenHash;
        }

        public async Task<bool> ValidateRefreshTokenAsync(string token)
        {
            return await _refreshTokenRepository.IsTokenValidAsync(token);
        }

        public async Task RevokeRefreshTokenAsync(string token)
        {
            var refreshToken = await _refreshTokenRepository.GetByTokenHashAsync(token);
            if (refreshToken != null)
            {
                await _refreshTokenRepository.RevokeTokenAsync(refreshToken.RefreshTokenId);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
