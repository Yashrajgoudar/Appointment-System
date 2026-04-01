using AuthService.Application.Common.Exceptions;
using AuthService.Application.Common.Interfaces;
using AuthService.Application.Features.Auth.DTOs;
using AuthService.Domain.Interfaces.Repositories;
using MediatR;

namespace AuthService.Application.Features.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;

        public RefreshTokenCommandHandler(
            IUnitOfWork unitOfWork,
            ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
        }

        public async Task<AuthResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            // Validate refresh token
            if (!await _tokenService.ValidateRefreshTokenAsync(request.RefreshToken))
            {
                throw new UnauthorizedException("Invalid or expired refresh token.");
            }

            // Get token from database
            var token = await _unitOfWork.RefreshTokens.GetByTokenHashAsync(request.RefreshToken);
            if (token == null || token.User == null)
            {
                throw new UnauthorizedException("Invalid refresh token.");
            }

            // Get user with roles
            var user = await _unitOfWork.Users.GetByEmailWithRolesAsync(token.User.Email);
            if (user == null || !user.IsActive)
            {
                throw new UnauthorizedException("User not found or inactive.");
            }

            // Revoke old refresh token
            await _tokenService.RevokeRefreshTokenAsync(request.RefreshToken);

            // Get user roles
            var roles = user.UserRoles?.Select(ur => ur.Role.RoleName).ToList() ?? new List<string>();

            // Generate new tokens
            var newAccessToken = _tokenService.GenerateAccessToken(user, roles);
            var newRefreshToken = await _tokenService.GenerateRefreshTokenAsync(user.UserId);

            var expirationMinutes = 15;
            var expiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes);

            return new AuthResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                ExpiresAt = expiresAt,
                User = new UserDto
                {
                    UserId = user.UserId,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    TenantId = user.TenantId,
                    TenantName = user.Tenant?.TenantName ?? "",
                    Roles = roles,
                    IsActive = user.IsActive,
                    IsEmailConfirmed = user.IsEmailConfirmed
                }
            };
        }
    }
}
