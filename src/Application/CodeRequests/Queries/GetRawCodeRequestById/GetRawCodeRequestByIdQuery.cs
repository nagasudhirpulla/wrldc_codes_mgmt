using Application.Common.Interfaces;
using Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CodeRequests.Queries.GetRawCodeRequestById;

public class GetRawCodeRequestByIdQuery : IRequest<CodeRequest?>
{
    public int CodeReqId { get; set; }

    public class GetRawCodeRequestByIdQueryHandler : IRequestHandler<GetRawCodeRequestByIdQuery, CodeRequest?>
    {
        private readonly IAppDbContext _context;

        public GetRawCodeRequestByIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<CodeRequest?> Handle(GetRawCodeRequestByIdQuery request, CancellationToken cancellationToken)
        {
            CodeRequest? res = await _context.CodeRequests
                 .Where(s => s.Id == request.CodeReqId)
                 .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            return res;
        }
    }
}
