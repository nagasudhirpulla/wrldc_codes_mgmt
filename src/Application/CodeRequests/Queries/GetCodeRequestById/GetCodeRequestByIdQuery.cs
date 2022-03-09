using Application.Common.Interfaces;
using Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CodeRequests.Queries.GetCodeRequestById;
public class GetCodeRequestByIdQuery : IRequest<CodeRequest?>
{
    public int CodeReqId { get; set; }

    public class GetCodeRequestByIdQueryHandler : IRequestHandler<GetCodeRequestByIdQuery, CodeRequest?>
    {
        private readonly IAppDbContext _context;

        public GetCodeRequestByIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<CodeRequest?> Handle(GetCodeRequestByIdQuery request, CancellationToken cancellationToken)
        {
            CodeRequest? res = await _context.CodeRequests
                 .Where(s => s.Id == request.CodeReqId)
                 .Include(s => s.Requester)
                 .Include(s => s.ElementOwners)
                 .Include(s => s.ConcernedStakeholders)
                 .ThenInclude(s => s.Stakeholder)
                 .Include(s => s.ConsentRequests)
                 .ThenInclude(s => s.Stakeholder)
                 .Include(s => s.RemarksRequests)
                 .ThenInclude(s => s.Stakeholder)
                 .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            return res;
        }
    }
}
