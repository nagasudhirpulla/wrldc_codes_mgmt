using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.CodeRequests.Commands.DeleteCodeRequest;

public class DeleteCodeRequestCommandHandler : IRequestHandler<DeleteCodeRequestCommand, List<string>>
{
    private readonly ILogger<DeleteCodeRequestCommandHandler> _logger;
    private readonly IAppDbContext _context;

    public DeleteCodeRequestCommandHandler(ILogger<DeleteCodeRequestCommandHandler> logger, IAppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<List<string>> Handle(DeleteCodeRequestCommand request, CancellationToken cancellationToken)
    {
        var req = await _context.CodeRequests.Where(c => c.Id == request.Id).FirstOrDefaultAsync(cancellationToken);
        if (req == null)
        {
            string errorMsg = $"Code Request Id {request.Id} not present for deletion";
            _logger.LogError(errorMsg);
            return new List<string>() { errorMsg };
        }

        _context.CodeRequests.Remove(req);
        await _context.SaveChangesAsync(cancellationToken);

        return new List<string>();
    }
}
