using Application.CodeRequestStakeholders.Commands.DeleteCodeReqStakeHolder;
using Application.CodeRequestStakeholders.Queries.GetRawCodeReqStakeHolder;
using Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Extensions;

namespace WebApp.Pages.CodeReqStakeHolders;

public class DeleteModel : PageModel
{
    // TODO manual test this
    private readonly ILogger<DeleteModel> _logger;
    private readonly IMediator _mediator;

    public DeleteModel(ILogger<DeleteModel> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [BindProperty]
    public CodeRequestStakeHolder? CodeReqStakeHolder { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        CodeReqStakeHolder = await _mediator.Send(new GetRawCodeReqStakeHolderQuery(id));
        if (CodeReqStakeHolder == null)
        {
            return NotFound();
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (CodeReqStakeHolder == null)
        {
            return NotFound();
        }

        CodeReqStakeHolder = await _mediator.Send(new GetRawCodeReqStakeHolderQuery(CodeReqStakeHolder.Id));
        if (CodeReqStakeHolder == null)
        {
            return NotFound();
        }

        List<string> errs = await _mediator.Send(new DeleteCodeReqStakeholderCommand() { Id = CodeReqStakeHolder.Id });

        if (errs.Count == 0)
        {
            _logger.LogInformation("Code Request Stakeholder deleted successfully");
            return RedirectToPage("/CodeRequests/Edit", new { id = CodeReqStakeHolder.CodeRequestId }).WithSuccess("Code Request Stakeholder deletion done");
        }

        foreach (var error in errs)
        {
            ModelState.AddModelError(string.Empty, error);
        }

        return Page();
    }
}
