using MediatR;

namespace AuthService.Application.Features.Auth.Commands.Logout
{
    public class LogoutCommand : IRequest<Unit>
    {
        public Guid UserId { get; init; }
        public string RefreshToken { get; init; } = default!;
    }
}
