using Core.ReportingData;
using Core.ReportingData.ElementsForDisplay;
using MediatR;

namespace Application.ReportingData.Queries.GetElementForDisplay;

public class GetAllGeneratingUnitsQuery : IRequest<List<ReportingGeneratingUnit>>
{
    public class GetAllGeneratingUnitsQueryHandler : IRequestHandler<GetAllGeneratingUnitsQuery, List<ReportingGeneratingUnit>>
    {
        private readonly IReportingDataService _reportingService;

        public GetAllGeneratingUnitsQueryHandler(IReportingDataService reportingService)
        {
            _reportingService = reportingService;
        }

        public async Task<List<ReportingGeneratingUnit>> Handle(GetAllGeneratingUnitsQuery request, CancellationToken cancellationToken)
        {
            List<ReportingGeneratingUnit> generatingUnits = _reportingService.GetAllGeneratingUnits();
            return await Task.FromResult(generatingUnits);
        }
    }
}
