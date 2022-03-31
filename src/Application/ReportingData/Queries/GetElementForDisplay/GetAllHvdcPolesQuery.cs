using Core.ReportingData;
using Core.ReportingData.ElementsForDisplay;
using MediatR;

namespace Application.ReportingData.Queries.GetElementForDisplay;

public class GetAllHvdcPolesQuery : IRequest<List<ReportingHvdcPole>>
{
    public class GetAllHvdcPolesQueryHandler : IRequestHandler<GetAllHvdcPolesQuery, List<ReportingHvdcPole>>
    {
        private readonly IReportingDataService _reportingService;

        public GetAllHvdcPolesQueryHandler(IReportingDataService reportingService)
        {
            _reportingService = reportingService;
        }

        public async Task<List<ReportingHvdcPole>> Handle(GetAllHvdcPolesQuery request, CancellationToken cancellationToken)
        {
            List<ReportingHvdcPole> hvdcPoles = _reportingService.GetAllHvdcPoles();
            return await Task.FromResult(hvdcPoles);
        }
    }
}
