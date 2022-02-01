using Application.Common.Interfaces;
using Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Users.Queries.IsUsrSelfOrAdmin;

public class IsUsrSelfOrAdminQuery : IRequest<bool>
{
    public string UsrId { get; set; }
    public class IsUsrSelfOrAdminQueryHandler : IRequestHandler<IsUsrSelfOrAdminQuery, bool>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<ApplicationUser> _userManager;

        public IsUsrSelfOrAdminQueryHandler(ICurrentUserService currentUserService, UserManager<ApplicationUser> userManager)
        {
            _currentUserService = currentUserService;
            _userManager = userManager;
        }
        public async Task<bool> Handle(IsUsrSelfOrAdminQuery request, CancellationToken cancellationToken)
        {
            string? curUsrId = _currentUserService.UserId;
            if (curUsrId == null || request.UsrId == null)
            {
                return false;
            }

            ApplicationUser curUsr = await _userManager.FindByIdAsync(curUsrId);
            IList<string> usrRoles = await _userManager.GetRolesAsync(curUsr);

            if (curUsrId == request.UsrId
                || usrRoles.Contains(SecurityConstants.AdminRoleString))
            {
                return true;
            }
            return false;
        }
    }

}
