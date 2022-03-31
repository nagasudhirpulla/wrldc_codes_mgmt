using Core.ReportingData;
using Core.ReportingData.ElementsForDisplay;
using MediatR;

namespace Application.ReportingData.Queries.ElementsForDisplay;

public class GetAllHvdcLineCktsQuery : IRequest<List<ReportingHvdcLineCkt>>
{
    public class GetAllHvdcLineCktsQueryHandler : IRequestHandler<GetAllHvdcLineCktsQuery, List<ReportingHvdcLineCkt>>
    {
        private readonly IReportingDataService _reportingService;

        public GetAllHvdcLineCktsQueryHandler(IReportingDataService reportingService)
        {
            _reportingService = reportingService;
        }

        public async Task<List<ReportingHvdcLineCkt>> Handle(GetAllHvdcLineCktsQuery request, CancellationToken cancellationToken)
        {
            List<ReportingHvdcLineCkt> hvdcLineCkts = _reportingService.GetAllHvdcLineCkts();
            return await Task.FromResult(hvdcLineCkts);
        }
    }
}
