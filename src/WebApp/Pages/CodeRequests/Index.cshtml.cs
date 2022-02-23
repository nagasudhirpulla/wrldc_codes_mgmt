using Application.CodeRequests.Queries.GetCodeRequestsBetweenDates;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.CodeRequests;

public class IndexModel : PageModel
{
    // TODO complete this
    private readonly ILogger<IndexModel> _logger;
    private readonly IMediator _mediator;
    public IList<CodeRequestDTO> ReqList { get; set; } = new List<CodeRequestDTO>();

    public IndexModel(ILogger<IndexModel> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task OnGetAsync()
    {
        ReqList = (await _mediator.Send(new GetCodeRequestsBetweenDatesQuery()));
    }
}
