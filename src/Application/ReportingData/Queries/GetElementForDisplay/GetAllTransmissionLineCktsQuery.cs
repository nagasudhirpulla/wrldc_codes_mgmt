using Core.ReportingData;
using Core.ReportingData.ElementsForDisplay;
using MediatR;

namespace Application.ReportingData.Queries.GetElementForDisplay;

public class GetAllTransmissionLineCktsQuery : IRequest<List<ReportingTransmissionLineCkt>>
{
    public class GetAllTransmissionLineCktsQueryHandler : IRequestHandler<GetAllTransmissionLineCktsQuery, List<ReportingTransmissionLineCkt>>
    {
        private readonly IReportingDataService _reportingService;

        public GetAllTransmissionLineCktsQueryHandler(IReportingDataService reportingService)
        {
            _reportingService = reportingService;
        }

        public async Task<List<ReportingTransmissionLineCkt>> Handle(GetAllTransmissionLineCktsQuery request, CancellationToken cancellationToken)
        {
            List<ReportingTransmissionLineCkt> transmissionLineCkts = _reportingService.GetAllTransmissionLineCkts();
            return await Task.FromResult(transmissionLineCkts);
        }
    }
}
