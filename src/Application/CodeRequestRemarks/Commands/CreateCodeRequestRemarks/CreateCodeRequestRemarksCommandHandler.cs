using Application.Common.Interfaces;
using AutoMapper;
using Core.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CodeRequestRemarks.Commands.CreateCodeRequestRemarks;

public class CreateCodeRequestRemarksCommandHandler : IRequestHandler<CreateCodeRequestRemarksCommand, List<string>>
{
    private readonly ILogger<CreateCodeRequestRemarksCommandHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IAppDbContext _context;

    public CreateCodeRequestRemarksCommandHandler(ILogger<CreateCodeRequestRemarksCommandHandler> logger, IMapper mapper, IAppDbContext context)
    {
        _logger = logger;
        _mapper = mapper;
        _context = context;
    }

    public async Task<List<string>> Handle(CreateCodeRequestRemarksCommand request, CancellationToken cancellationToken)
    {
        List<string> errs = new();
        CodeRequestRemark crremarks = _mapper.Map<CodeRequestRemark>(request);
        _context.CodeRequestRemarks.Add(crremarks);
        _ = await _context.SaveChangesAsync(cancellationToken);
        return errs;
    }
}
