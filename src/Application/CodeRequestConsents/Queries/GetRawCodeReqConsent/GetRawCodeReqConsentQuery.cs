using Application.Common.Interfaces;
using Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Application.CodeRequestConsents.Queries.GetRawCodeReqConsent;

public class GetRawCodeReqConsentQuery : IRequest<CodeRequestConsent?>
{
    public int Id { get; set; }
    public GetRawCodeReqConsentQuery(int id)
    {
        Id = id;
    }

    public class GetRawCodeReqConsentQueryHandler : IRequestHandler<GetRawCodeReqConsentQuery, CodeRequestConsent?>
    {
        private readonly IAppDbContext _context;

        public GetRawCodeReqConsentQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<CodeRequestConsent?> Handle(GetRawCodeReqConsentQuery request, CancellationToken cancellationToken)
        {
            // get list of user Ids associated with code request
            CodeRequestConsent? crs = await _context.CodeRequestConsents
                                .Where(x => x.Id == request.Id)
                                .FirstOrDefaultAsync(cancellationToken: cancellationToken);



            return crs;
        }
    }
}