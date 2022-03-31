using Core.ReportingData;
using Core.ReportingData.ElementsForDisplay;
using MediatR;

namespace Application.ReportingData.Queries.ElementsForDisplay;

public class GetAllBusReactorsQuery : IRequest<List<ReportingBusReactor>>
{
    public class GetAllBusReactorsQueryHandler : IRequestHandler<GetAllBusReactorsQuery, List<ReportingBusReactor>>
    {
        private readonly IReportingDataService _reportingService;

        public GetAllBusReactorsQueryHandler(IReportingDataService reportingService)
        {
            _reportingService = reportingService;
        }

        public async Task<List<ReportingBusReactor>> Handle(GetAllBusReactorsQuery request, CancellationToken cancellationToken)
        {
            List<ReportingBusReactor> busReactors = _reportingService.GetAllBusReactors();
            return await Task.FromResult(busReactors);
        }
    }
}
