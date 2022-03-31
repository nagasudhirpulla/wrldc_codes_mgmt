using Core.ReportingData;
using Core.ReportingData.ElementsForDisplay;
using MediatR;

namespace Application.ReportingData.Queries.GetElementForDisplay;

public class GetAllCompensatorsQuery : IRequest<List<ReportingCompensator>>
{
    public class GetAllCompensatorsQueryHandler : IRequestHandler<GetAllCompensatorsQuery, List<ReportingCompensator>>
    {
        private readonly IReportingDataService _reportingService;

        public GetAllCompensatorsQueryHandler(IReportingDataService reportingService)
        {
            _reportingService = reportingService;
        }

        public async Task<List<ReportingCompensator>> Handle(GetAllCompensatorsQuery request, CancellationToken cancellationToken)
        {
            List<ReportingCompensator> compensators = _reportingService.GetAllCompensators();
            return await Task.FromResult(compensators);
        }
    }
}
