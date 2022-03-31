using Core.ReportingData;
using Core.ReportingData.ElementsForDisplay;
using MediatR;

namespace Application.ReportingData.Queries.GetElementForDisplay;

public class GetAllFscsQuery : IRequest<List<ReportingFsc>>
{
    public class GetAllFscsQueryHandler : IRequestHandler<GetAllFscsQuery, List<ReportingFsc>>
    {
        private readonly IReportingDataService _reportingService;

        public GetAllFscsQueryHandler(IReportingDataService reportingService)
        {
            _reportingService = reportingService;
        }

        public async Task<List<ReportingFsc>> Handle(GetAllFscsQuery request, CancellationToken cancellationToken)
        {
            List<ReportingFsc> fscs = _reportingService.GetAllFscs();
            return await Task.FromResult(fscs);
        }
    }
}
