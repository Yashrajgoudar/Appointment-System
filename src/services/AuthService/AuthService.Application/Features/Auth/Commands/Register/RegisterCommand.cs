using AuthService.Application.Features.Auth.DTOs;
using MediatR;

namespace AuthService.Application.Features.Auth.Commands.Register
{
    public class RegisterCommand : IRequest<AuthResponse>
    {
        public string Email { get; init; } = default!;
        public string Password { get; init; } = default!;
        public string FirstName { get; init; } = default!;
        public string LastName { get; init; } = default!;
        public string PhoneNumber { get; init; } = default!;
        public Guid TenantId { get; init; }
    }
}
