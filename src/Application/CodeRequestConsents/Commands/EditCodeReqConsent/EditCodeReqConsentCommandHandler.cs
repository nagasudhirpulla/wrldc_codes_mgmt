using Application.Common.Interfaces;
using Application.Users;
using Core.Entities;
using Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.CodeRequestConsents.Commands.EditCodeReqConsent;
public class EditCodeReqConsentCommandHandler : IRequestHandler<EditCodeReqConsentCommand, List<string>>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly UserManager<ApplicationUser> _userManager;

    public EditCodeReqConsentCommandHandler(IAppDbContext context, ICurrentUserService currentUserService, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _currentUserService = currentUserService;
        _userManager = userManager;
    }
    public async Task<List<string>> Handle(EditCodeReqConsentCommand request, CancellationToken cancellationToken)
    {
        List<string> errs = new();
        var consentReq = await _context.CodeRequestConsents
                 .Where(s => s.Id == request.Id).Include(s => s.CodeRequest)
                 .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        if (consentReq == null)
        {
            errs.Add("Consent Request Id not valid");
            return errs;

        }
        // check if user is the remarks stakeholder or admin
        string? curUsrId = _currentUserService.UserId;
        ApplicationUser curUsr = await _userManager.FindByIdAsync(curUsrId);
        var isUsrAdminOrRldc = (await _userManager.GetRolesAsync(curUsr))
                                .Any(x => new List<string>() { SecurityConstants.AdminRoleString, SecurityConstants.RldcRoleString }.Contains(x));

        if (!((consentReq.StakeholderId == curUsrId) || isUsrAdminOrRldc))
        {
            return new List<string>() { $"User is not allowed to edit this Consent Request Id {request.Id}" };
        }

        // check if action taken already on code request
        bool isCodeReqActionTaken = new List<CodeRequestStatus> { CodeRequestStatus.DisApproved, CodeRequestStatus.Approved }.Contains(consentReq.CodeRequest!.RequestState);
        if (isCodeReqActionTaken)
        {
            return new List<string>() { $"Code request of Consent Request Id {request.Id} is already approved or dis-approved" };
        }

        // edit the consent request
        consentReq.Remarks = request.Remarks;
        consentReq.ApprovalStatus = request.ApprovalStatus;
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.CodeRequestConsents.Any(e => e.Id == request.Id))
            {
                return new List<string>() { $"Consent Request Id {request.Id} not present for editing" };
            }
            else
            {
                throw;
            }
        }
        return errs;
    }
}
