using Application.Common.Interfaces;
using AutoMapper;
using Core.Entities;
using Core.ReportingData;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.CodeRequests.Commands.CreateOutageRevivalCodeRequest;

public class CreateOutageRevivalCodeRequestCommandHandler : IRequestHandler<CreateOutageRevivalCodeRequestCommand, List<string>>
{
    private readonly ILogger<CreateOutageRevivalCodeRequestCommandHandler> _logger;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;
    private readonly IAppDbContext _context;
    private readonly IReportingDataService _reportingDataService;

    public CreateOutageRevivalCodeRequestCommandHandler(ILogger<CreateOutageRevivalCodeRequestCommandHandler> logger, IMapper mapper, ICurrentUserService currentUserService, IAppDbContext context, IReportingDataService reportingDataService)
    {
        _logger = logger;
        _mapper = mapper;
        _context = context;
        _currentUserService = currentUserService;
        _reportingDataService = reportingDataService;
    }

    public async Task<List<string>> Handle(CreateOutageRevivalCodeRequestCommand request, CancellationToken cancellationToken)
    {
        List<string> errs = new();

        // TODO complete this
        
        // fetch outage entry from reporting database by request outage Id
        // if outage entry not present, send error

        // get the owners of the element associated with the ouatge using element Id and element type
        // check if the logged in user has one of the element owners map to his login
        // if atleast one of element owners not mapped with login user, send error

        // check if the element is already revived (element is revived if revived time is not null)
        // if element is already revived, send error

        // check if a outage revival code request (which is not dis-approved) is already generated for this outage ID
        // if duplicate entry exists, send error

        // if all checks passed, then persist the revival code request to the database

        return errs;
    }
}

