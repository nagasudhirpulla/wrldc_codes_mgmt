using MediatR;
using Microsoft.AspNetCore.Identity;
using Core.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Users.Commands.SeedUsers;

public class SeedUsersCommand : IRequest<bool>
{
    public class SeedUsersCommandHandler : IRequestHandler<SeedUsersCommand, bool>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IdentityInit _identityInit;
        private readonly ILogger<SeedUsersCommandHandler> _logger;

        public SeedUsersCommandHandler(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IdentityInit identityInit, ILogger<SeedUsersCommandHandler> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _identityInit = identityInit;
            _logger = logger;
        }

        public async Task<bool> Handle(SeedUsersCommand request, CancellationToken cancellationToken)
        {
            await SeedUserRoles();
            await SeedUsers();
            return true;
        }

        /**
         * This method seeds admin and guest users
         * **/
        public async Task SeedUsers()
        {
            //int deptId = (await _context.Departments.Where(d => d.Name.ToLower() == "na").FirstAsync()).Id;
            //int desigId = (await _context.Designations.Where(d => d.Name.ToLower() == "na").FirstAsync()).Id;
            await SeedUser(_identityInit.AdminUserName, _identityInit.AdminEmail,
                _identityInit.AdminPassword, SecurityConstants.AdminRoleString);
            await SeedUser(_identityInit.GuestUserName, _identityInit.GuestEmail,
                _identityInit.GuestPassword, SecurityConstants.StakeholderRoleString);
        }

        /**
         * This method seeds a user
         * **/
        public async Task SeedUser(string userName, string email, string password, string role)
        {
            // check if user doesn't exist
            if ((_userManager.FindByNameAsync(userName).Result) == null)
            {
                // create desired user object
                ApplicationUser user = new()
                {
                    UserName = userName,
                    Email = email,
                };

                // push desired user object to DB
                IdentityResult result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    _ = await _userManager.AddToRoleAsync(user, role);
                    // confirm user email
                    string emailResetToken = await _userManager.GenerateChangeEmailTokenAsync(user, email);
                    IdentityResult emailChangeResult = await _userManager.ChangeEmailAsync(user, email, emailResetToken);
                    if (emailChangeResult.Succeeded)
                    {
                        _logger.LogInformation("seed user email confirmed!");
                    }
                    else
                    {
                        _logger.LogInformation("seed user email not confirmed...");
                    }
                }
            }
        }

        /**
         * This method seeds roles
         * **/
        public async Task SeedUserRoles()
        {
            foreach (string r in SecurityConstants.GetRoles())
            {
                await SeedRole(r);
            }
        }

        /**
         * This method seeds a role
         * **/
        public async Task SeedRole(string roleString)
        {
            // check if role doesn't exist
            if (!(_roleManager.RoleExistsAsync(roleString).Result))
            {
                // create desired role object
                IdentityRole role = new()
                {
                    Name = roleString,
                };
                // push desired role object to DB
                _ = await _roleManager.CreateAsync(role);
            }
        }
    }
}
