using Core.ReportingData;
using MediatR;

namespace Application.ReportingData.Queries.GetApprovedOutagesForDate;

public class GetApprovedOutagesForDateQuery : IRequest<List<ReportingOutageRequest>>
{
    public DateTime ReqDt { get; set; }
    public class GetApprovedOutagesForRequesterQueryHandler : IRequestHandler<GetApprovedOutagesForDateQuery, List<ReportingOutageRequest>>
    {
        private readonly IReportingDataService _reportingService;

        public GetApprovedOutagesForRequesterQueryHandler(IReportingDataService reportingService)
        {
            _reportingService = reportingService;
        }

        public async Task<List<ReportingOutageRequest>> Handle(GetApprovedOutagesForDateQuery request, CancellationToken cancellationToken)
        {
            List<ReportingOutageRequest> approvedOutages = _reportingService.GetApprovedOutageRequestsForDate(request.ReqDt);
            return await Task.FromResult(approvedOutages);
        }
    }
}
