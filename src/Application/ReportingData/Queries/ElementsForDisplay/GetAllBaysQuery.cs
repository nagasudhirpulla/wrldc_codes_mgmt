using Core.ReportingData;
using Core.ReportingData.ElementsForDisplay;
using MediatR;

namespace Application.ReportingData.Queries.ElementsForDisplay;

public class GetAllBaysQuery : IRequest<List<ReportingBay>>
{
    public class GetAllBaysQueryHandler : IRequestHandler<GetAllBaysQuery, List<ReportingBay>>
    {
        private readonly IReportingDataService _reportingService;

        public GetAllBaysQueryHandler(IReportingDataService reportingService)
        {
            _reportingService = reportingService;
        }

        public async Task<List<ReportingBay>> Handle(GetAllBaysQuery request, CancellationToken cancellationToken)
        {
            List<ReportingBay> bays = _reportingService.GetAllBays();
            return await Task.FromResult(bays);
        }
    }
}
