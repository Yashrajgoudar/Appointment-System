namespace AuthService.Application.Features.Auth.DTOs
{
    public record RefreshTokenRequest
    {
        public string RefreshToken { get; init; } = default!;
    }
}
