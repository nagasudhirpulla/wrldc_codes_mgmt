using Application.Common.Interfaces;
using Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Application.CodeRequestRemarks.Queries.GetRawCodeRequestRemark;

public class GetRawCodeRequestRemarkQuery : IRequest<CodeRequestRemark?>
{
    public int Id { get; set; }
    public GetRawCodeRequestRemarkQuery(int id)
    {
        Id = id;
    }

    public class GetRawCodeReqConsentQueryHandler : IRequestHandler<GetRawCodeRequestRemarkQuery, CodeRequestRemark?>
    {
        private readonly IAppDbContext _context;

        public GetRawCodeReqConsentQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<CodeRequestRemark?> Handle(GetRawCodeRequestRemarkQuery request, CancellationToken cancellationToken)
        {
            // get list of user Ids associated with code request
            CodeRequestRemark? crs = await _context.CodeRequestRemarks
                                .Where(x => x.Id == request.Id)
                                .FirstOrDefaultAsync(cancellationToken: cancellationToken);



            return crs;
        }
    }
}