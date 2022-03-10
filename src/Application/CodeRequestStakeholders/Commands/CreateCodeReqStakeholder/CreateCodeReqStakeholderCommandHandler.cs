using Application.Common.Interfaces;
using AutoMapper;
using Core.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.CodeRequestStakeholders.Commands.CreateCodeReqStakeholder;

public class CreateCodeReqStakeholderCommandHandler : IRequestHandler<CreateCodeReqStakeholderCommand, List<string>>
{
    private readonly ILogger<CreateCodeReqStakeholderCommandHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IAppDbContext _context;

    public CreateCodeReqStakeholderCommandHandler(ILogger<CreateCodeReqStakeholderCommandHandler> logger, IMapper mapper, IAppDbContext context)
    {
        _logger = logger;
        _mapper = mapper;
        _context = context;
    }

    public async Task<List<string>> Handle(CreateCodeReqStakeholderCommand request, CancellationToken cancellationToken)
    {
        List<string> errs = new();
        CodeRequestStakeHolder crs = _mapper.Map<CodeRequestStakeHolder>(request);
        _context.CodeRequestStakeHolders.Add(crs);
        _ = await _context.SaveChangesAsync(cancellationToken);
        return errs;
    }
}

