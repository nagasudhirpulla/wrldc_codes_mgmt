using Application.Common.Interfaces;
using Core.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CodeRequests.Commands.UpdateCodeReqFromCodeBook;

public class UpdateCodeReqFromCodeBookCommandHandler : IRequestHandler<UpdateCodeReqFromCodeBookCommand, List<string>>
{
    private readonly IAppDbContext _context;
    public UpdateCodeReqFromCodeBookCommandHandler(IAppDbContext context)
    {
        _context = context;
    }
    public async Task<List<string>> Handle(UpdateCodeReqFromCodeBookCommand request, CancellationToken cancellationToken)
    {
        // TODO complete this
        List<string> errs = new();
        var codeReq = await _context.CodeRequests
                 .Where(s => s.Id == request.CodeReqId)
                 .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        if (codeReq == null)
        {
            errs.Add("Code Request Id not valid");
            return errs;
        }
        if (request.IsApproved)
        {
            codeReq.Code = request.Code;
            codeReq.RequestState= CodeRequestStatus.Approved;
        }

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.CodeRequests.Any(e => e.Id == request.CodeReqId))
            {
                return new List<string>() { $"Code Request Id {request.CodeReqId} not present for editing" };
            }
            else
            {
                throw;
            }
        }
        return errs;
    }
}