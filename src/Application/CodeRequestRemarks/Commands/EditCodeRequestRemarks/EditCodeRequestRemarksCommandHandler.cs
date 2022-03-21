using Application.Common.Interfaces;
using Application.Users;
using AutoMapper;
using Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CodeRequestRemarks.Commands.EditCodeRequestRemarks;

public class EditCodeRequestRemarksCommandHandler : IRequestHandler<EditCodeRequestRemarksCommand, List<string>>
{
    private readonly ILogger<EditCodeRequestRemarksCommandHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly UserManager<ApplicationUser> _userManager;

    public EditCodeRequestRemarksCommandHandler(ILogger<EditCodeRequestRemarksCommandHandler> logger, IMapper mapper, IAppDbContext context, ICurrentUserService currentUserService, UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _mapper = mapper;
        _context = context;
        _currentUserService = currentUserService;
        _userManager = userManager;

    }

    public async Task<List<string>> Handle(EditCodeRequestRemarksCommand request, CancellationToken cancellationToken)
    {
        var crRemarks = await _context.CodeRequestRemarks
            .Where(coderemark => coderemark.Id == request.Id).Include(s => s.CodeRequest)
            .FirstOrDefaultAsync(cancellationToken);
        if (crRemarks == null)
        {
            string errorMsg = $"Code Remark Id {request.Id} not present for editing";
            return new List<string>() { errorMsg };
        }
        string? curUsrId = _currentUserService.UserId;
        ApplicationUser curUsr = await _userManager.FindByIdAsync(curUsrId);
        var isUsrAdminOrRldc = (await _userManager.GetRolesAsync(curUsr))
                                .Any(x => new List<string>() { SecurityConstants.AdminRoleString, SecurityConstants.RldcRoleString }.Contains(x));
        bool isStakeholderOrAdmin = false;
        if ((crRemarks.StakeholderId == curUsrId) || (isUsrAdminOrRldc))
        {
            isStakeholderOrAdmin = true;
        }
        if (!isStakeholderOrAdmin)
        {
            return new List<string>() { $"User is not allowed to edit this Consent Request Id {request.Id}" };
        }
        // check if user is the remarks stakeholder or admin
        if (crRemarks.Remarks != request.Remarks)
        {
            crRemarks.Remarks = request.Remarks;
        }
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.CodeRequestRemarks.Any(e => e.Id == request.Id))
            {
                return new List<string>() { $"Code Remark Id {request.Id} not present for editing" };
            }
            else
            {
                throw;
            }
        }
        return new List<string>();
    }
}
