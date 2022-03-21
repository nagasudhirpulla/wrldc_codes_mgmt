using Application.Common.Interfaces;
using Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CodeRequestStakeholders.Queries.GetRawCodeReqStakeHolder;

public class GetRawCodeReqStakeHolderQuery : IRequest<CodeRequestStakeHolder?>
{
    public int Id { get; set; }
    public GetRawCodeReqStakeHolderQuery(int id)
    {
        Id = id;
    }

    public class GetRawCodeReqStakeHolderQueryHandler : IRequestHandler<GetRawCodeReqStakeHolderQuery, CodeRequestStakeHolder?>
    {
        private readonly IAppDbContext _context;

        public GetRawCodeReqStakeHolderQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<CodeRequestStakeHolder?> Handle(GetRawCodeReqStakeHolderQuery request, CancellationToken cancellationToken)
        {
            // get list of user Ids associated with code request
            CodeRequestStakeHolder? crs = await _context.CodeRequestStakeHolders
                                .Where(x => x.Id == request.Id)
                                .FirstOrDefaultAsync(cancellationToken: cancellationToken);



            return crs;
        }
    }
}
