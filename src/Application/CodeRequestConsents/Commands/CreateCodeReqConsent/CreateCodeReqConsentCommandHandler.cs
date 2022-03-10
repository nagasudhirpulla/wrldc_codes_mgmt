using Application.Common.Interfaces;
using AutoMapper;
using Core.Entities;
using Core.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.CodeRequestConsents.Commands.CreateCodeReqConsent;

public class CreateCodeReqConsentCommandHandler : IRequestHandler<CreateCodeReqConsentCommand, List<string>>
{
    private readonly ILogger<CreateCodeReqConsentCommandHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IAppDbContext _context;

    public CreateCodeReqConsentCommandHandler(ILogger<CreateCodeReqConsentCommandHandler> logger, IMapper mapper, IAppDbContext context)
    {
        _logger = logger;
        _mapper = mapper;
        _context = context;
    }

    public async Task<List<string>> Handle(CreateCodeReqConsentCommand request, CancellationToken cancellationToken)
    {
        List<string> errs = new();
        CodeRequestConsent crs = _mapper.Map<CodeRequestConsent>(request);
        crs.ApprovalStatus = ApprovalStatus.Pending;
        _context.CodeRequestConsents.Add(crs);
        _ = await _context.SaveChangesAsync(cancellationToken);
        return errs;
    }
}