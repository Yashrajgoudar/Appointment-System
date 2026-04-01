using AuthService.Application.Features.Auth.DTOs;

namespace AuthService.Application.Common.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(RegisterRequest request);
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task<AuthResponse> RefreshTokenAsync(string refreshToken);
        Task LogoutAsync(Guid userId, string refreshToken);
        Task<UserDto> GetCurrentUserAsync(Guid userId);
    }
}
