using Core.ReportingData;
using Core.ReportingData.ElementsForDisplay;
using MediatR;

namespace Application.ReportingData.Queries.GetElementForDisplay;

public class GetAllBusesQuery : IRequest<List<ReportingBus>>
{
    public class GetAllBusesQueryHandler : IRequestHandler<GetAllBusesQuery, List<ReportingBus>>
    {
        private readonly IReportingDataService _reportingService;

        public GetAllBusesQueryHandler(IReportingDataService reportingService)
        {
            _reportingService = reportingService;
        }

        public async Task<List<ReportingBus>> Handle(GetAllBusesQuery request, CancellationToken cancellationToken)
        {
            List<ReportingBus> buses = _reportingService.GetAllBuses();
            return await Task.FromResult(buses);
        }
    }
}
