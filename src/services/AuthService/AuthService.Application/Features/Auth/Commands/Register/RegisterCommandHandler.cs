using AuthService.Application.Common.Exceptions;
using AuthService.Application.Common.Interfaces;
using AuthService.Application.Features.Auth.DTOs;
using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces.Repositories;
using MediatR;

namespace AuthService.Application.Features.Auth.Commands.Register
{
    public class RegisterCommandHandler :IRequestHandler<RegisterCommand,AuthResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordService _passwordService;
        private readonly ITokenService _tokenService;

        public RegisterCommandHandler(
            IUnitOfWork unitOfWork,
            IPasswordService passwordService,
            ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _passwordService = passwordService;
            _tokenService = tokenService;
        }

        public async Task<AuthResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            // Check if email already exists
            if (await _unitOfWork.Users.EmailExistsAsync(request.Email))
            {
                throw new ValidationException("Email already exists.");
            }

            // Validate password strength
            if (!_passwordService.ValidatePasswordStrength(request.Password))
            {
                throw new ValidationException(
                    "Password must be at least 8 characters and contain uppercase, lowercase, and numbers.");
            }

            // Check if tenant exists
            var tenant = await _unitOfWork.Tenants.GetByIdAsync(request.TenantId);
            if (tenant == null)
            {
                throw new ValidationException("Invalid tenant.");
            }

            // Hash password using PBKDF2
            var (passwordHash, passwordSalt) = _passwordService.HashPassword(request.Password);

            // Create user
            var user = new User
            {
                UserId = Guid.NewGuid(),
                TenantId = request.TenantId,
                Email = request.Email.ToLower(),
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                IsEmailConfirmed = false,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Users.AddAsync(user);

            // Assign default "Customer" role
            var customerRole = await _unitOfWork.Roles.GetByNameAsync("Customer");
            if (customerRole != null)
            {
                await _unitOfWork.UserRoles.AssignRoleToUserAsync(user.UserId, customerRole.RoleId);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Generate tokens
            var roles = new List<string> { "Customer" };
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
                    TenantName = tenant.TenantName,
                    Roles = roles,
                    IsActive = user.IsActive,
                    IsEmailConfirmed = user.IsEmailConfirmed
                }
            };
        }
    }
}
