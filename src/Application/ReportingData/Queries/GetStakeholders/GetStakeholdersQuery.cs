using Core.ReportingData;
using MediatR;

namespace Application.ReportingData.Queries.GetStakeholders;


public class GetStakeholdersQuery : IRequest<List<ReportingStakeholder>>
{
    public class GetStakeholdersQueryHandler : IRequestHandler<GetStakeholdersQuery, List<ReportingStakeholder>>
    {
        private readonly IReportingDataService _reportingService;

        public GetStakeholdersQueryHandler(IReportingDataService reportingService)
        {
            _reportingService = reportingService;
        }

        public async Task<List<ReportingStakeholder>> Handle(GetStakeholdersQuery request, CancellationToken cancellationToken)
        {
            List<ReportingStakeholder> stakeholders = _reportingService.GetReportingStakeHolders();
            return stakeholders;
        }
    }
}
