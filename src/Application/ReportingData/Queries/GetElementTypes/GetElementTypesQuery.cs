using Core.ReportingData;
using MediatR;

namespace Application.ReportingData.Queries.GetElementTypes;

public class GetElementTypesQuery : IRequest<List<ElementType>>
{
    public class GetElementTypesQueryHandler : IRequestHandler<GetElementTypesQuery, List<ElementType>>
    {
        private readonly IReportingDataService _reportingService;

        public GetElementTypesQueryHandler(IReportingDataService reportingService)
        {
            _reportingService = reportingService;
        }

        public async Task<List<ElementType>> Handle(GetElementTypesQuery request, CancellationToken cancellationToken)
        {
            List<ElementType> elType = _reportingService.GetElementTypes();
            return await Task.FromResult(elType);
        }
    }
}
