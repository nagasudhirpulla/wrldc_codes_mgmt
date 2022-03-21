using Application.Common.Interfaces;
using AutoMapper;
using Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CodeRequestConsents.Queries.GetCodeRequestConsentsBetweenDates;

public class GetCodeRequestConsentsBetweenDatesQuery : IRequest<List<CodeRequestConsent>>
{
    public DateTime StartDate { get; set; } = DateTime.Now;
    public DateTime EndDate { get; set; } = DateTime.Now;

    public class GetCodeRequestConsentsBetweenDatesQueryHandler : IRequestHandler<GetCodeRequestConsentsBetweenDatesQuery, List<CodeRequestConsent>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public GetCodeRequestConsentsBetweenDatesQueryHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<CodeRequestConsent>> Handle(GetCodeRequestConsentsBetweenDatesQuery request, CancellationToken cancellationToken)
        {
            DateTime startDt = new(request.StartDate.Year, request.StartDate.Month, request.StartDate.Day);
            DateTime endDt = new(request.EndDate.Year, request.EndDate.Month, request.EndDate.Day, 23, 59, 59);
            List<CodeRequestConsent> consentRequests = await _context.CodeRequestConsents
                .Where(s => (s.Created >= startDt && s.Created <= endDt))
                .Include(s => s.CodeRequest)
                .Include(s => s.Stakeholder)
                .ToListAsync(cancellationToken: cancellationToken);

            return consentRequests;
        }
    }
}
