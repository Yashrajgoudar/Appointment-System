using AuthService.Application.Features.Auth.DTOs;
using MediatR;

namespace AuthService.Application.Features.Auth.Commands.Login
{
    public class LoginCommand : IRequest<AuthResponse>
    {
        public string Email { get; init; } = default!;
        public string Password { get; init; } = default!;
    }
}
