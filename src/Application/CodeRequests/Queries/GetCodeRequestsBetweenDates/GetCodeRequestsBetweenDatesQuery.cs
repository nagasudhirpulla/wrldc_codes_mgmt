using Application.Common.Interfaces;
using AutoMapper;
using Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CodeRequests.Queries.GetCodeRequestsBetweenDates;

public class GetCodeRequestsBetweenDatesQuery : IRequest<List<CodeRequestDTO>>
{
    public DateTime StartDate { get; set; } = DateTime.Now;
    public DateTime EndDate { get; set; } = DateTime.Now;
    public class GetCodeRequestsBetweenDatesQueryHandler : IRequestHandler<GetCodeRequestsBetweenDatesQuery, List<CodeRequestDTO>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public GetCodeRequestsBetweenDatesQueryHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<CodeRequestDTO>> Handle(GetCodeRequestsBetweenDatesQuery request, CancellationToken cancellationToken)
        {
            var endDt = new DateTime(request.EndDate.Year, request.EndDate.Month, request.EndDate.Day, 23, 59, 59);
            List<CodeRequest> reqList = await _context.CodeRequests
                .Where(s => (s.Created >= request.StartDate && s.Created <= endDt) ||
                        (s.DesiredExecutionStartTime >= request.StartDate && s.DesiredExecutionStartTime <= endDt))
                .ToListAsync(cancellationToken: cancellationToken);
            List<CodeRequestDTO> res = reqList.Select(r => _mapper.Map<CodeRequestDTO>(r)).ToList();
            return res;
        }
    }
}
