using Application.Common.Interfaces;
using AutoMapper;
using Core.Entities;
using Core.Enums;
using Core.ReportingData;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.CodeRequests.Commands.CreateElementOutageCodeRequest;

public class CreateElementOutageCodeRequestCommandHandler : IRequestHandler<CreateElementOutageCodeRequestCommand, List<string>>
{
    private readonly ILogger<CreateElementOutageCodeRequestCommandHandler> _logger;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;
    private readonly IAppDbContext _context;
    private readonly IReportingDataService _reportingDataService;

    public CreateElementOutageCodeRequestCommandHandler(ILogger<CreateElementOutageCodeRequestCommandHandler> logger, IMapper mapper, ICurrentUserService currentUserService, IAppDbContext context, IReportingDataService reportingDataService)
    {
        _logger = logger;
        _mapper = mapper;
        _context = context;
        _currentUserService = currentUserService;
        _reportingDataService = reportingDataService;
    }

    public async Task<List<string>> Handle(CreateElementOutageCodeRequestCommand request, CancellationToken cancellationToken)
    {
        List<string> errs = new();
        string? curUsrId = _currentUserService.UserId;
        // create new code request entity from command
        CodeRequest newCodeReq = _mapper.Map<CodeRequest>(request);
        // set code type as element outage code
        newCodeReq.CodeType = CodeType.Outage;

        newCodeReq.RequestState = CodeRequestStatus.Requested;
        // set logged in user as the requester
        newCodeReq.RequesterId = curUsrId;
        string? elType = request.ElementType;
        int elId = request.ElementId;

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
            _logger.LogError("rolling back DB transaction while element outage code request creation, {message}", ex.Message);
            throw;
        }
        return errs;
    }
}
