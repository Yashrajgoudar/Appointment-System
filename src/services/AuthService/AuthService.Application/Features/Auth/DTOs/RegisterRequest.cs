namespace AuthService.Application.Features.Auth.DTOs
{
    public record RegisterRequest
    {
        public string Email { get; init; } = default!;
        public string Password { get; init; } = default!;
        public string FirstName { get; init; } = default!;
        public string LastName { get; init; } = default!;
        public string? PhoneNumber { get; init; }
        public Guid TenantId { get; init; }
    }
}
