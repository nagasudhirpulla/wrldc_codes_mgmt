using Application.CodeReqRemarks.Commands.DeleteCodeReqRemarks;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CodeRequestRemarks.Commands.DeleteCodeRequestRemarks;

internal class DeleteCodeRequestRemarksCommandHandler : IRequestHandler<DeleteCodeRequestRemarksCommand, List<string>>
{
    private readonly ILogger<DeleteCodeRequestRemarksCommandHandler> _logger;
    private readonly IAppDbContext _context;

    public DeleteCodeRequestRemarksCommandHandler(ILogger<DeleteCodeRequestRemarksCommandHandler> logger, IAppDbContext context)
    {
        _logger = logger;
        _context = context;
    }
    public async Task<List<string>> Handle(DeleteCodeRequestRemarksCommand request, CancellationToken cancellationToken)
    {
        var req = await _context.CodeRequestRemarks.Where(c => c.Id == request.Id).FirstOrDefaultAsync(cancellationToken);
        if (req == null)
        {
            string errorMsg = $"Code request remarks with Id {request.Id} not present for deletion";
            _logger.LogError(errorMsg);
            return new List<string>() { errorMsg };
        }

        _context.CodeRequestRemarks.Remove(req);
        await _context.SaveChangesAsync(cancellationToken);

        return new List<string>();
    }
}
