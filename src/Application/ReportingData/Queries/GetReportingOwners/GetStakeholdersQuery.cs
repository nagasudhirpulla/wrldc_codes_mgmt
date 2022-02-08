using Core.ReportingData;
using MediatR;

namespace Application.ReportingData.Queries.GetReportingOwners;


public class GetOwnersQuery : IRequest<List<ReportingOwner>>
{
    public class GetOwnersQueryHandler : IRequestHandler<GetOwnersQuery, List<ReportingOwner>>
    {
        private readonly IReportingDataService _reportingService;

        public GetOwnersQueryHandler(IReportingDataService reportingService)
        {
            _reportingService = reportingService;
        }

        public async Task<List<ReportingOwner>> Handle(GetOwnersQuery request, CancellationToken cancellationToken)
        {
            List<ReportingOwner> stakeholders = _reportingService.GetReportingOwners();
            return await Task.FromResult(stakeholders);
        }
    }
}
