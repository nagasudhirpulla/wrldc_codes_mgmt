using Application.Common.Interfaces;
using AutoMapper;
using Core.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CodeRequestRemarks.Queries.GetCodeRequestRemarksBetweenDates;

public class GetCodeRequestRemarksBetweenDatesQuery : IRequest<List<CodeRequestRemark>>
{
    public DateTime StartDate { get; set; } = DateTime.Now;
    public DateTime EndDate { get; set; } = DateTime.Now;

    public class GetCodeRequestRemarksBetweenDatesQueryHandler : IRequestHandler<GetCodeRequestRemarksBetweenDatesQuery, List<CodeRequestRemark>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public GetCodeRequestRemarksBetweenDatesQueryHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<CodeRequestRemark>> Handle(GetCodeRequestRemarksBetweenDatesQuery request, CancellationToken cancellationToken)
        {
            DateTime startDt = new(request.StartDate.Year, request.StartDate.Month, request.StartDate.Day);
            DateTime endDt = new(request.EndDate.Year, request.EndDate.Month, request.EndDate.Day, 23, 59, 59);
            List<CodeRequestRemark> reqList = await _context.CodeRequestRemarks
                .Where(s => s.Created >= startDt && s.Created <= endDt)
                .Include(s => s.CodeRequest)
                .Include(s => s.Stakeholder)
                .OrderByDescending(x => x.Created)
                .ToListAsync(cancellationToken: cancellationToken);
            return reqList;
        }
    }
}
