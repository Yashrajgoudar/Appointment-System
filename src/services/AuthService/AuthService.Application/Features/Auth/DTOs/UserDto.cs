namespace AuthService.Application.Features.Auth.DTOs
{
    public record UserDto
    {
        public Guid UserId { get; init; }
        public string Email { get; init; } = default!;
        public string FirstName { get; init; } = default!;
        public string LastName { get; init; } = default!;
        public string? PhoneNumber { get; init; }
        public Guid TenantId { get; init; }
        public string TenantName { get; init; } = default!;
        public IEnumerable<string> Roles { get; init; } = new List<string>();
        public bool IsActive { get; init; }
        public bool IsEmailConfirmed { get; init; }
    }
}
