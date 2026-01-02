namespace AuthService.Application.Features.Auth.DTOs
{
    public record LoginRequest
    {
        public string Email { get; init; } = default!;
        public string Password { get; init; } = default!;
    }
}
