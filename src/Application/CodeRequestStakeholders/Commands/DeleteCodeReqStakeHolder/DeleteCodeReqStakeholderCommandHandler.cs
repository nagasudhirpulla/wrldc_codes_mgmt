using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.CodeRequestStakeholders.Commands.DeleteCodeReqStakeHolder;

public class DeleteCodeReqStakeholderCommandHandler : IRequestHandler<DeleteCodeReqStakeholderCommand, List<string>>
{
    private readonly ILogger<DeleteCodeReqStakeholderCommandHandler> _logger;
    private readonly IAppDbContext _context;

    public DeleteCodeReqStakeholderCommandHandler(ILogger<DeleteCodeReqStakeholderCommandHandler> logger, IAppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<List<string>> Handle(DeleteCodeReqStakeholderCommand request, CancellationToken cancellationToken)
    {
        var req = await _context.CodeRequestStakeHolders.Where(c => c.Id == request.Id).FirstOrDefaultAsync(cancellationToken);
        if (req == null)
        {
            string errorMsg = $"Code request stakeholder with Id {request.Id} not present for deletion";
            _logger.LogError(errorMsg);
            return new List<string>() { errorMsg };
        }

        _context.CodeRequestStakeHolders.Remove(req);
        await _context.SaveChangesAsync(cancellationToken);

        return new List<string>();
    }
}
