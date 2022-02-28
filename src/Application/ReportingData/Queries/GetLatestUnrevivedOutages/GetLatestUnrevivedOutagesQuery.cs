using Core.ReportingData;
using MediatR;

namespace Application.ReportingData.Queries.GetLatestUnrevivedOutages;

public class GetLatestUnrevivedOutagesQuery : IRequest<List<ReportingUnrevivedOutage>>
{
    public DateTime ReqDt { get; set; }
    public class GetLatestUnrevivedOutagesQueryHandler : IRequestHandler<GetLatestUnrevivedOutagesQuery, List<ReportingUnrevivedOutage>>
    {
        private readonly IReportingDataService _reportingService;

        public GetLatestUnrevivedOutagesQueryHandler(IReportingDataService reportingService)
        {
            _reportingService = reportingService;
        }

        public async Task<List<ReportingUnrevivedOutage>> Handle(GetLatestUnrevivedOutagesQuery request, CancellationToken cancellationToken)
        {
            List<ReportingUnrevivedOutage> latestUnrevOutages = _reportingService.GetLatestUnrevivedOutages();
            return await Task.FromResult(latestUnrevOutages);
        }
    }
}
