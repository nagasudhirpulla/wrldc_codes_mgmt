using Application.CodeRequests.Queries.GetCodeRequestsBetweenDates;
using CodeBookApi.Infra.BasicAuth;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace CodeBookApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CodeRequestsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CodeRequestsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public IList<CodeRequestDTO> GetCodeReqs { get; set; } = new List<CodeRequestDTO>();

    [BasicAuth] // You can optionally provide a specific realm --> [BasicAuth("my-realm")]
    [HttpGet(Name = "GetCodes")]
    public async Task<IList<CodeRequestDTO>> GetAsync([FromQuery] string? startDt, [FromQuery] string? endDt)
    {
        string format = "yyyy-MM-dd";
        DateTime startDate = DateTime.ParseExact(startDt ?? "", format, CultureInfo.InvariantCulture);
        DateTime endDate = DateTime.ParseExact(endDt ?? "", format, CultureInfo.InvariantCulture);
        GetCodeReqs = await _mediator.Send(new GetCodeRequestsBetweenDatesQuery() { StartDate = startDate, EndDate = endDate });

        return GetCodeReqs;
    }
}
