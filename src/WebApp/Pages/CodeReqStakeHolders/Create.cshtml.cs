using Application.CodeRequestStakeholders.Commands.CreateCodeReqStakeholder;
using Application.CodeRequestStakeholders.Queries.GetUnmappedStakeHolders;
using Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Extensions;

namespace WebApp.Pages.CodeReqStakeHolders;

public class CreateModel : PageModel
{
    private readonly ILogger<CreateModel> _logger;
    private readonly IMediator _mediator;

    [BindProperty]
    public CreateCodeReqStakeholderCommand NewStakeHolder { get; set; }

    public SelectList? UnmappedStakeHolders { get; set; }

    public CreateModel(ILogger<CreateModel> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }
    public async Task<IActionResult> OnGetAsync(int id)
    {
        await InitSelectListItems(id);
        NewStakeHolder = new() { CodeRequestId = id };
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (NewStakeHolder == null)
        {
            return NotFound();
        }
        List<string> errs = await _mediator.Send(NewStakeHolder);

        if (errs != null && errs.Count == 0)
        {
            return RedirectToPage("/CodeRequests/Edit", new { id = NewStakeHolder.CodeRequestId }).WithSuccess("Code Request Stakeholder added");
        }

        foreach (var error in errs!)
        {
            ModelState.AddModelError(string.Empty, error);
        }

        await InitSelectListItems(NewStakeHolder.CodeRequestId);

        return Page();
    }

    public async Task InitSelectListItems(int codeReqId)
    {
        List<ApplicationUser>? usrs = await _mediator.Send(new GetUnmappedStakeHoldersQuery(codeReqId));
        UnmappedStakeHolders = new SelectList(usrs, "Id", "DisplayName");
    }
}
