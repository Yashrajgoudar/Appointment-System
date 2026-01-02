using AuthService.Application.Features.Auth.DTOs;
using MediatR;

namespace AuthService.Application.Features.Auth.Commands.GetCurrentUser
{
    public class GetCurrentUserQuery : IRequest<UserDto>
    {
        public Guid UserId { get; init; }
    }
}
