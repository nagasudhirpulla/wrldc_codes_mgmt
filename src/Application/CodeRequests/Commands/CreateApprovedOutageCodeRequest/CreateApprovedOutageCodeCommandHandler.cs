using Application.Common.Interfaces;
using AutoMapper;
using Core.Entities;
using Core.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.CodeRequests.Commands.CreateApprovedOutageCodeRequest;

public class CreateApprovedOutageCodeRequestCommandHandler : IRequestHandler<CreateApprovedOutageCodeRequestCommand, List<string>>
{
    private readonly ILogger<CreateApprovedOutageCodeRequestCommandHandler> _logger;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;
    private readonly IAppDbContext _context;

    public CreateApprovedOutageCodeRequestCommandHandler(ILogger<CreateApprovedOutageCodeRequestCommandHandler> logger, IMapper mapper, ICurrentUserService currentUserService, IAppDbContext context)
    {
        _logger = logger;
        _mapper = mapper;
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<List<string>> Handle(CreateApprovedOutageCodeRequestCommand request, CancellationToken cancellationToken)
    {
        List<string> result = new();

        // create new code request entity from command
        CodeRequest newCodeReq = _mapper.Map<CodeRequest>(request);

        // TODO populate all the code request properties from the approved outage request

        // TODO derive concerened stake holder user logins and attach to the code request

        // set code type as approved outage code
        newCodeReq.CodeType = CodeType.ApprovedOutage;

        // set logged in user as the requester
        newCodeReq.RequesterId = _currentUserService.UserId;

        // insert row into the code requests table
        _context.CodeRequests.Add(newCodeReq);

        // persist changes to database
        _ = await _context.SaveChangesAsync(cancellationToken);

        // TODO create code request creation event and event handler to send notifications to the RLDC users
        return result;
    }
}