using Application.Common.Interfaces;
using AutoMapper;
using Core.Entities;
using Core.Enums;
using Core.ReportingData;
using MediatR;
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

    public CreateApprovedOutageCodeRequestCommandHandler(ILogger<CreateApprovedOutageCodeRequestCommandHandler> logger, IMapper mapper, ICurrentUserService currentUserService, IAppDbContext context, IReportingDataService reportingDataService)
    {
        _logger = logger;
        _mapper = mapper;
        _context = context;
        _currentUserService = currentUserService;
        _reportingDataService = reportingDataService;
    }

    public async Task<List<string>> Handle(CreateApprovedOutageCodeRequestCommand request, CancellationToken cancellationToken)
    {
        List<string> errs = new();

        // create new code request entity from command
        CodeRequest newCodeReq = _mapper.Map<CodeRequest>(request);

        ReportingOutageRequest? outageReq = _reportingDataService.GetApprovedOutageRequestById(request.ApprovedOutageRequestId);
        if (outageReq == null)
        {
            // custom exception can be created instead
            errs.Add("Approved Outage Request Id not valid");
            return errs;
        }
        ReportingOutageRequest req = outageReq!;

        // check if any valid approved outage code request is already created
        bool isCodeReqAlreadyCreated = await _context.CodeRequests.AnyAsync(c => (c.OutageRequestId == req.ShutdownRequestId) && (c.RequestState != CodeRequestStatus.DisApproved), cancellationToken: cancellationToken);
        if (isCodeReqAlreadyCreated)
        {
            // custom exception can be created instead
            errs.Add("Code request for this approved outage already created");
            return errs;
        }

        // check if the requester of the outage request if mapped with the logged in user
        string? curUsrId = _currentUserService.UserId;
        bool isOutageRequesterValid = await _context.UserStakeholders.AnyAsync(us => (us.StakeHolderId == req.RequesterId) && (us.UsrId == curUsrId), cancellationToken: cancellationToken);
        if (!isOutageRequesterValid)
        {
            // custom exception can be created instead
            errs.Add("Approved Outage requester id is not mapped to the current logged in user");
            return errs;
        }

        // check if approved outage request date range is already over
        DateTime nowTime = DateTime.Now;
        bool isOutageRequestStale = req.ApprovedEndTime < nowTime;
        if (isOutageRequestStale)
        {
            // custom exception can be created instead
            errs.Add("Outage request date range already elapsed");
            return errs;
        }

        // set code type as approved outage code
        newCodeReq.CodeType = CodeType.ApprovedOutage;

        newCodeReq.RequestState = CodeRequestStatus.Requested;

        // set logged in user as the requester
        newCodeReq.RequesterId = curUsrId;

        string? elType = req.ElementType;
        int elId = req.ElementId;

        // populate all the code request properties from the approved outage request
        newCodeReq.Description = req.Reason;

        List<string> remarks = new();
        if (string.IsNullOrWhiteSpace(req.RldcRemarks))
        {
            remarks.Add(req.RldcRemarks!);
        }
        if (string.IsNullOrWhiteSpace(req.NldcRemarks))
        {
            remarks.Add("NLDC remarks - " + req.NldcRemarks);
        }

        newCodeReq.Remarks = (remarks.Count == 0) ? null : string.Join(". ", remarks);

        newCodeReq.ElementId = elId;
        newCodeReq.ElementName = req.ElementName;

        newCodeReq.ElementTypeId = req.ElementTypeId;
        newCodeReq.ElementType = elType;

        newCodeReq.OutageTypeId = req.OutageTypeId;
        newCodeReq.OutageType = req.OutageType;

        newCodeReq.OutageTag = req.OutageTag;
        newCodeReq.OutageTagId = req.OutageTagId;

        newCodeReq.OutageRequestId = req.ShutdownRequestId;

        newCodeReq.DesiredExecutionStartTime = req.ApprovedStartTime;
        newCodeReq.DesiredExecutionEndTime = req.ApprovedEndTime;

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
            _logger.LogError("rolling back DB transaction while creting approved outage code request creation, {message}", ex.Message);
            throw;
        }

        return errs;
    }
}