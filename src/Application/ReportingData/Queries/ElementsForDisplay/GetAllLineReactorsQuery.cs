using Core.ReportingData;
using Core.ReportingData.ElementsForDisplay;
using MediatR;

namespace Application.ReportingData.Queries.ElementsForDisplay;

public class GetAllLineReactorsQuery : IRequest<List<ReportingLineReactor>>
{
    public class GetAllLineReactorsQueryHandler : IRequestHandler<GetAllLineReactorsQuery, List<ReportingLineReactor>>
    {
        private readonly IReportingDataService _reportingService;

        public GetAllLineReactorsQueryHandler(IReportingDataService reportingService)
        {
            _reportingService = reportingService;
        }

        public async Task<List<ReportingLineReactor>> Handle(GetAllLineReactorsQuery request, CancellationToken cancellationToken)
        {
            List<ReportingLineReactor> lineReactors = _reportingService.GetAllLineReactors();
            return await Task.FromResult(lineReactors);
        }
    }
}
