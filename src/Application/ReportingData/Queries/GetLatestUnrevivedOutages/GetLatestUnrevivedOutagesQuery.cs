using Core.ReportingData;
using MediatR;

namespace Application.ReportingData.Queries.GetLatestUnrevivedOutages;

public class GetLatestUnrevivedOutagesQuery : IRequest<List<ReportingOutage>>
{
    public DateTime ReqDt { get; set; }
    public class GetLatestUnrevivedOutagesQueryHandler : IRequestHandler<GetLatestUnrevivedOutagesQuery, List<ReportingOutage>>
    {
        private readonly IReportingDataService _reportingService;

        public GetLatestUnrevivedOutagesQueryHandler(IReportingDataService reportingService)
        {
            _reportingService = reportingService;
        }

        public async Task<List<ReportingOutage>> Handle(GetLatestUnrevivedOutagesQuery request, CancellationToken cancellationToken)
        {
            List<ReportingOutage> latestUnrevOutages = _reportingService.GetLatestUnrevivedOutages();
            return await Task.FromResult(latestUnrevOutages);
        }
    }
}
