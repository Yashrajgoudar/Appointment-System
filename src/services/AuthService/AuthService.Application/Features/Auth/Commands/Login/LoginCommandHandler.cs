using AuthService.Application.Common.Exceptions;
using AuthService.Application.Common.Interfaces;
using AuthService.Application.Features.Auth.DTOs;
using AuthService.Domain.Interfaces.Repositories;
using MediatR;

namespace AuthService.Application.Features.Auth.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordService _passwordService;
        private readonly ITokenService _tokenService;

        public LoginCommandHandler(
            IUnitOfWork unitOfWork,
            IPasswordService passwordService,
            ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _passwordService = passwordService;
            _tokenService = tokenService;
        }

        public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // Get user with roles
            var user = await _unitOfWork.Users.GetByEmailWithRolesAsync(request.Email);

            if (user == null)
            {
                throw new UnauthorizedException("Invalid email or password.");
            }

            // Verify password using PBKDF2
            if (!_passwordService.VerifyPassword(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                throw new UnauthorizedException("Invalid email or password.");
            }

            // Check if user is active
            if (!user.IsActive)
            {
                throw new UnauthorizedException("Account is deactivated.");
            }

            // Update last login
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Get user roles
            var roles = user.UserRoles?.Select(ur => ur.Role.RoleName).ToList() ?? new List<string>();

            // Generate tokens
            var accessToken = _tokenService.GenerateAccessToken(user, roles);
            var refreshToken = await _tokenService.GenerateRefreshTokenAsync(user.UserId);

            var expirationMinutes = 15;
            var expiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes);

            return new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
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
