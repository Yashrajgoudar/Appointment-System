using AuthService.Application.Common.Exceptions;
using AuthService.Application.Features.Auth.DTOs;
using AuthService.Domain.Interfaces.Repositories;
using MediatR;

namespace AuthService.Application.Features.Auth.Commands.GetCurrentUser
{
    public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, UserDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCurrentUserQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UserDto> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(request.UserId);
            if (user == null)
            {
                throw new UnauthorizedException("User not found.");
            }

            var roles = await _unitOfWork.UserRoles.GetRolesByUserIdAsync(request.UserId);
            var roleNames = roles.Select(r => r.RoleName).ToList();

            var tenant = await _unitOfWork.Tenants.GetByIdAsync(user.TenantId);

            return new UserDto
            {
                UserId = user.UserId,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                TenantId = user.TenantId,
                TenantName = tenant?.TenantName ?? "",
                Roles = roleNames,
                IsActive = user.IsActive,
                IsEmailConfirmed = user.IsEmailConfirmed
            };
        }
    }
}
