using AuthService.Application.Common.Interfaces;
using MediatR;

namespace AuthService.Application.Features.Auth.Commands.Logout
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Unit>
    {
        private readonly ITokenService _tokenService;

        public LogoutCommandHandler(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            await _tokenService.RevokeRefreshTokenAsync(request.RefreshToken);
            return Unit.Value;
        }
    }
}
