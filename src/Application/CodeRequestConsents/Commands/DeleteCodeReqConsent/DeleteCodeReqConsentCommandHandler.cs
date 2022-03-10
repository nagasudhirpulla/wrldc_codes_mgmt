using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.CodeRequestConsents.Commands.DeleteCodeReqConsent;

public class DeleteCodeReqConsentCommandHandler : IRequestHandler<DeleteCodeReqConsentCommand, List<string>>
{
    private readonly ILogger<DeleteCodeReqConsentCommandHandler> _logger;
    private readonly IAppDbContext _context;

    public DeleteCodeReqConsentCommandHandler(ILogger<DeleteCodeReqConsentCommandHandler> logger, IAppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<List<string>> Handle(DeleteCodeReqConsentCommand request, CancellationToken cancellationToken)
    {
        var req = await _context.CodeRequestConsents.Where(c => c.Id == request.Id).FirstOrDefaultAsync(cancellationToken);
        if (req == null)
        {
            string errorMsg = $"Code request Consents with Id {request.Id} not present for deletion";
            _logger.LogError(errorMsg);
            return new List<string>() { errorMsg };
        }

        _context.CodeRequestConsents.Remove(req);
        await _context.SaveChangesAsync(cancellationToken);

        return new List<string>();
    }
}
