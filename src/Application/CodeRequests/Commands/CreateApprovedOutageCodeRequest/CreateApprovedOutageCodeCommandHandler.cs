using Application.Common.Interfaces;
using AutoMapper;
using Core.Entities;
using Core.Enums;
using Core.ReportingData;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.CodeRequests.Commands.CreateApprovedOutageCodeRequest;

public class CreateApprovedOutageCodeRequestCommandHandler : IRequestHandler<CreateApprovedOutageCodeRequestCommand, List<string>>
{
    private readonly ILogger<CreateApprovedOutageCodeRequestCommandHandler> _logger;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;
    private readonly IAppDbContext _context;
    private readonly IReportingDataService _reportingDataService;
    private readonly UserManager<ApplicationUser> _userManager;

    public CreateApprovedOutageCodeRequestCommandHandler(ILogger<CreateApprovedOutageCodeRequestCommandHandler> logger, IMapper mapper, ICurrentUserService currentUserService, IAppDbContext context, IReportingDataService reportingDataService, UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _mapper = mapper;
        _context = context;
        _currentUserService = currentUserService;
        _reportingDataService = reportingDataService;
        _userManager = userManager;
    }

    public async Task<List<string>> Handle(CreateApprovedOutageCodeRequestCommand request, CancellationToken cancellationToken)
    {
        List<string> result = new();

        // create new code request entity from command
        CodeRequest newCodeReq = _mapper.Map<CodeRequest>(request);

        ReportingOutageRequest? outageReq = _reportingDataService.GetApprovedOutageRequestById(request.ApprovedOutageRequestId);
        if (outageReq == null)
        {
            // custom exception can be created instead
            result.Add("Approved Outage Request Id not valid");
        }
        ReportingOutageRequest req = outageReq!;

        // check if approved outage request date range is already over
        DateTime nowTime = DateTime.Now;
        bool isOutageRequestStale = req.ApprovedEndTime < nowTime;
        if (isOutageRequestStale)
        {
            // custom exception can be created instead
            result.Add("Outage request date range already elapsed");
        }

        // set code type as approved outage code
        newCodeReq.CodeType = CodeType.ApprovedOutage;

        newCodeReq.RequestState = CodeRequestStatus.Requested;

        // set logged in user as the requester
        newCodeReq.RequesterId = _currentUserService.UserId;

        // populate all the code request properties from the approved outage request
        newCodeReq.Description = req.Reason;

        newCodeReq.ElementId = req.ElementId;
        newCodeReq.ElementName = req.ElementName;

        newCodeReq.ElementTypeId = req.ElementTypeId;
        newCodeReq.ElementType = req.ElementType;

        newCodeReq.OutageTypeId = req.OutageTypeId;
        newCodeReq.OutageType = req.OutageType;

        newCodeReq.OutageTag = req.OutageTag;
        newCodeReq.OutageTagId = req.OutageTagId;

        newCodeReq.OutageApprovalId = req.ShutdownRequestId;

        newCodeReq.DesiredExecutionStartTime = req.ApprovedStartTime;
        newCodeReq.DesiredExecutionEndTime = req.ApprovedEndTime;

        // insert row into the code requests table
        _context.CodeRequests.Add(newCodeReq);

        // TODO derive element owners and attach to code request
        List<CodeRequestElementOwner> elementOwners = new();

        // Derive concerened stake holder login users based on owners
        List<CodeRequestStakeHolder> concernedUsers = await _context.UserElementOwners
                    .Where(uo => elementOwners.Any(eo => uo.OwnerId == eo.OwnerId))
                    .Select(uo => new CodeRequestStakeHolder { CodeRequestId = newCodeReq.Id, StakeholderId = uo.UsrId })
                    .ToListAsync(cancellationToken: cancellationToken);

        // link the concerened stakeholders with the code
        _context.CodeRequestStakeHolders.AddRange(concernedUsers);

        // persist changes to database
        _ = await _context.SaveChangesAsync(cancellationToken);

        // TODO create code request creation event and event handler to send notifications to the RLDC users
        return result;
    }
}