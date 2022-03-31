using Core.ReportingData;
using Core.ReportingData.ElementsForDisplay;
using MediatR;

namespace Application.ReportingData.Queries.GetElementForDisplay;

public class GetAllTransformersQuery : IRequest<List<ReportingTransformer>>
{
    public class GetAllTransformersQueryHandler : IRequestHandler<GetAllTransformersQuery, List<ReportingTransformer>>
    {
        private readonly IReportingDataService _reportingService;

        public GetAllTransformersQueryHandler(IReportingDataService reportingService)
        {
            _reportingService = reportingService;
        }

        public async Task<List<ReportingTransformer>> Handle(GetAllTransformersQuery request, CancellationToken cancellationToken)
        {
            List<ReportingTransformer> transformer = _reportingService.GetAllTransformers();
            return await Task.FromResult(transformer);
        }
    }
}
