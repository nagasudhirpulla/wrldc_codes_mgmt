using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Core.Entities;
using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Commands.EditUser;

public class EditUserCommandHandler : IRequestHandler<EditUserCommand, List<string>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<EditUserCommandHandler> _logger;
    private readonly IAppDbContext _context;

    public EditUserCommandHandler(UserManager<ApplicationUser> userManager, ILogger<EditUserCommandHandler> logger, IAppDbContext appDbContext)
    {
        _userManager = userManager;
        _logger = logger;
        _context = appDbContext;
    }

    public async Task<List<string>> Handle(EditUserCommand request, CancellationToken cancellationToken)
    {
        List<string> errors = new();
        ApplicationUser user = await _userManager.FindByIdAsync(request.Id);
        if (user == null)
        {
            errors.Add($"Unable to find user with id {request.Id}");
        }
        List<IdentityError> identityErrors = new();
        // change password if not null
        string newPassword = request.Password;
        if (!string.IsNullOrWhiteSpace(newPassword))
        {
            string passResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            IdentityResult passResetResult = await _userManager.ResetPasswordAsync(user, passResetToken, newPassword);
            if (passResetResult.Succeeded)
            {
                _logger.LogInformation("User password changed");
            }
            else
            {
                identityErrors.AddRange(passResetResult.Errors);
            }
        }

        // change username if changed
        if (user.UserName != request.Username)
        {
            IdentityResult usernameChangeResult = await _userManager.SetUserNameAsync(user, request.Username);
            if (usernameChangeResult.Succeeded)
            {
                _logger.LogInformation("Username changed");

            }
            else
            {
                identityErrors.AddRange(usernameChangeResult.Errors);
            }
        }

        // change email if changed
        if (user.Email != request.Email)
        {
            string emailResetToken = await _userManager.GenerateChangeEmailTokenAsync(user, request.Email);
            IdentityResult emailChangeResult = await _userManager.ChangeEmailAsync(user, request.Email, emailResetToken);
            if (emailChangeResult.Succeeded)
            {
                _logger.LogInformation("email changed");
            }
            else
            {
                identityErrors.AddRange(emailChangeResult.Errors);
            }
        }

        // change phone number if changed
        if (user.PhoneNumber != request.PhoneNumber)
        {
            string phoneChangeToken = await _userManager.GenerateChangePhoneNumberTokenAsync(user, request.PhoneNumber);
            IdentityResult phoneChangeResult = await _userManager.ChangePhoneNumberAsync(user, request.PhoneNumber, phoneChangeToken);
            if (phoneChangeResult.Succeeded)
            {
                _logger.LogInformation("phone number of user {username} with id {userId} changed to {phone}", user.UserName, user.Id, request.PhoneNumber);
            }
            else
            {
                identityErrors.AddRange(phoneChangeResult.Errors);
            }
        }

        // change user role if not present in user
        bool isValidRole = SecurityConstants.GetRoles().Contains(request.UserRole);
        List<string> existingUserRoles = (await _userManager.GetRolesAsync(user)).ToList();
        bool isRoleChanged = !existingUserRoles.Any(r => r == request.UserRole);
        if (isValidRole)
        {
            if (isRoleChanged)
            {
                // remove existing user roles if any
                await _userManager.RemoveFromRolesAsync(user, existingUserRoles);
                // add new Role to user from VM
                await _userManager.AddToRoleAsync(user, request.UserRole);
            }
        }

        // check if two factor authentication to be changed
        if (user.TwoFactorEnabled != request.IsTwoFactorEnabled)
        {
            IdentityResult twoFactorChangeResult = await _userManager.SetTwoFactorEnabledAsync(user, request.IsTwoFactorEnabled);
            if (twoFactorChangeResult.Succeeded)
            {
                _logger.LogInformation("two factor enabled = {IsTwoFactorEnabled}", request.IsTwoFactorEnabled);
            }
            else
            {
                identityErrors.AddRange(twoFactorChangeResult.Errors);
            }
        }

        user.DisplayName = request.DisplayName;
        IdentityResult updateRes = await _userManager.UpdateAsync(user);
        if (!updateRes.Succeeded)
        {
            // TODO log this
            identityErrors.AddRange(updateRes.Errors);
        }

        // update the user stakeholders
        if (request.Stakeholders != null)
        {
            var updatedStakeholderIds = request.Stakeholders.Select(x => x.Id);

            // get existing stakeholders
            List<UserStakeholder> existingStakeholders = await _context.UserStakeholders.Where(uS => uS.UsrId == user.Id)
                                                                .ToListAsync(cancellationToken: cancellationToken);
            var existingStakeholderIds = existingStakeholders.Select(x => x.StakeHolderId);

            // find the user stakeholders to be inserted
            List<UserStakeholder> newUserStakeholders = request.Stakeholders
                                                        .Where(x => !existingStakeholderIds.Contains(x.Id))
                                                        .Select(x => new UserStakeholder()
                                                        {
                                                            UsrId = user.Id,
                                                            StakeHolderId = x.Id,
                                                            StakeHolderName = x.Username
                                                        }).ToList();


            // find the user stakeholders to be deleted
            List<UserStakeholder> toDeleteUserStakeholders = existingStakeholders
                                                            .Where(x => !updatedStakeholderIds.Contains(x.StakeHolderId))
                                                            .ToList();

            // persist the updates on user stakeholders
            _context.UserStakeholders.AddRange(newUserStakeholders);
            _context.UserStakeholders.RemoveRange(toDeleteUserStakeholders);
            _ = await _context.SaveChangesAsync(cancellationToken);
        }

        // update the user element owners
        if (request.ElementOwners != null)
        {
            var updatedElemOwnerIds = request.ElementOwners.Select(x => x.Id);

            // get existing element-owners
            List<UserElementOwner> existingElemOwners = await _context.UserElementOwners.Where(uS => uS.UsrId == user.Id)
                                                                .ToListAsync(cancellationToken: cancellationToken);
            var existingElemOwnerIds = existingElemOwners.Select(x => x.OwnerId);

            // find the user element-owners to be inserted
            List<UserElementOwner> newUserElemOwners = request.ElementOwners
                                                        .Where(x=> !existingElemOwnerIds.Contains(x.Id))
                                                        .Select(x=> new UserElementOwner()
                                                        {
                                                            UsrId = user.Id,
                                                            OwnerId = x.Id,
                                                            OwnerName = x.Name
                                                        }).ToList();
            
            // find the user element-owners to be deleted
            List<UserElementOwner> toDeleteUserElemOwners = existingElemOwners
                                                            .Where(x=> !updatedElemOwnerIds.Contains(x.OwnerId))
                                                            .ToList();
            // persist the updates on user stakeholders
            _context.UserElementOwners.AddRange(newUserElemOwners);
            _context.UserElementOwners.RemoveRange(toDeleteUserElemOwners);
            _ = await _context.SaveChangesAsync(cancellationToken);
        }

        foreach (IdentityError iError in identityErrors)
        {
            errors.Add(iError.Description);
        }
        return errors;
    }
}
