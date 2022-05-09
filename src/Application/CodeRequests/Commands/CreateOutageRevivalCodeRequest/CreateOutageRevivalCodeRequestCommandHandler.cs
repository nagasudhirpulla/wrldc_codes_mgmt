using Application.Common.Interfaces;
using Application.Users;
using AutoMapper;
using Core.Entities;
using Core.Enums;
using Core.ReportingData;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.CodeRequests.Commands.CreateOutageRevivalCodeRequest;

public class CreateOutageRevivalCodeRequestCommandHandler : IRequestHandler<CreateOutageRevivalCodeRequestCommand, List<string>>
{
    private readonly ILogger<CreateOutageRevivalCodeRequestCommandHandler> _logger;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;
    private readonly IAppDbContext _context;
    private readonly IReportingDataService _reportingDataService;
    private readonly UserManager<ApplicationUser> _userManager;

    public CreateOutageRevivalCodeRequestCommandHandler(ILogger<CreateOutageRevivalCodeRequestCommandHandler> logger, IMapper mapper, ICurrentUserService currentUserService, IAppDbContext context, IReportingDataService reportingDataService, UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _mapper = mapper;
        _context = context;
        _currentUserService = currentUserService;
        _reportingDataService = reportingDataService;
        _userManager = userManager;
    }

    public async Task<List<string>> Handle(CreateOutageRevivalCodeRequestCommand request, CancellationToken cancellationToken)
    {
        List<string> errs = new();

        // fetch outage entry from reporting database by request outage Id
        ReportingOutage? outage = _reportingDataService.GetLatestOutageById(request.OutageId);
        // if outage entry not present, send error
        if (outage == null)
        {
            errs.Add("Outage Id not valid");
            return errs;
        }

        string? curUsrId = _currentUserService.UserId;
        ApplicationUser curUsr = await _userManager.FindByIdAsync(curUsrId);
        var isUsrAdminOrRldc = (await _userManager.GetRolesAsync(curUsr))
                                .Any(x => new List<string>() { SecurityConstants.AdminRoleString, SecurityConstants.RldcRoleString }.Contains(x));

        if (!isUsrAdminOrRldc)
        {
            // get the owners of the element associated with the ouatge using element Id and element type
            List<ReportingOwner>? elOwners = _reportingDataService.GetElementOwners(outage!.ElementType!, outage.ElementId);

            // check if the logged in user has one of the element owners map to his login
            bool isElOwnerLinked = await _context.UserElementOwners.AnyAsync(ueo => elOwners.Select(eo => eo.Id).Contains(ueo.OwnerId), cancellationToken: cancellationToken);
            // if atleast one of element owners not mapped with login user, send error
            if (!isElOwnerLinked)
            {
                errs.Add("Atleast one element owner of the outage element is not linked to this logged in user");
                return errs;
            }
        }

        // check if the element is already revived (element is revived if revived time is not null)
        bool isElRevived = outage!.RevivalDateTime.HasValue;
        // if element is already revived, send error
        if (isElRevived)
        {
            errs.Add("Element is already revived");
            return errs;
        }

        // check if a outage revival code request (which is not dis-approved) is already generated for this outage ID
        bool isCodeReqAlreadyCreated = await _context.CodeRequests.AnyAsync(c => (c.OutageId == request.OutageId) && (c.RequestState != CodeRequestStatus.DisApproved), cancellationToken: cancellationToken);
        // if duplicate entry exists, send error
        if (isCodeReqAlreadyCreated)
        {
            errs.Add("Code request for this outage already created");
            return errs;
        }

        // create new code request entity from command
        CodeRequest newCodeReq = _mapper.Map<CodeRequest>(request);

        // set code type as approved outage code
        newCodeReq.CodeType = CodeType.Revival;

        newCodeReq.RequestState = CodeRequestStatus.Requested;

        // set logged in user as the requester
        newCodeReq.RequesterId = curUsrId;

        string? elType = outage.ElementType;
        int elId = outage.ElementId;

        newCodeReq.Description = request.Remarks ?? "Revival Code Request";

        newCodeReq.ElementId = elId;
        newCodeReq.ElementName = outage.ElementName;

        newCodeReq.ElementTypeId = outage.ElementTypeId;
        newCodeReq.ElementType = elType;

        newCodeReq.OutageTypeId = outage.OutageTypeId;
        newCodeReq.OutageType = outage.OutageType;

        newCodeReq.OutageTag = outage.OutageTag;
        newCodeReq.OutageTagId = outage.OutageTagId;

        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            // insert code request into the code requests table
            _context.CodeRequests.Add(newCodeReq);
            _ = await _context.SaveChangesAsync(cancellationToken);

            // derive element owners and attach to code request
            List<ReportingOwner> owners = _reportingDataService.GetElementOwners(elType!, elId);
            List<CodeRequestElementOwner> elementOwners = owners.Select(o => new CodeRequestElementOwner { CodeRequestId = newCodeReq.Id, OwnerId = o.Id, OwnerName = o.Name })
                                                            .ToList();
            _context.CodeRequestElementOwners.AddRange(elementOwners);

            // derive concerened stake holder login users based on owners
            List<CodeRequestStakeHolder> concernedUsers = await _context.UserElementOwners
                        .Where(uo => elementOwners.Select(eo => eo.OwnerId).Contains(uo.OwnerId))
                        .Select(uo => new CodeRequestStakeHolder { CodeRequestId = newCodeReq.Id, StakeholderId = uo.UsrId })
                        .ToListAsync(cancellationToken: cancellationToken);

            // link the concerened stakeholders with the code
            _context.CodeRequestStakeHolders.AddRange(concernedUsers);

            // persist changes to database
            _ = await _context.SaveChangesAsync(cancellationToken);

            // TODO create code request creation event and event handler to send notifications to the RLDC users

            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            _logger.LogError("rolling back DB transaction while revival code request creation, {message}", ex.Message);
            throw;
        }
        return errs;
    }
}

