using Application.CodeRequestRemarks.Commands.CreateCodeRequestRemarks;
using Application.CodeRequestStakeholders.Queries.GetUnmappedStakeHolders;
using Application.Users.Queries.GetAppUsers;
using Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Extensions;

namespace WebApp.Pages.CodeRequestRemarks;

public class CreateModel : PageModel
{
    private readonly ILogger<CreateModel> _logger;
    private readonly IMediator _mediator;

    [BindProperty]
    public CreateCodeRequestRemarksCommand NewRemark { get; set; }

    public SelectList? AllStakeHolders { get; set; }

    public CreateModel(ILogger<CreateModel> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }
    public async Task<IActionResult> OnGetAsync(int id)
    {
        await InitSelectListItems();
        NewRemark = new() { CodeRequestId = id };
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (NewRemark == null)
        {
            return NotFound();
        }
        List<string> errs = await _mediator.Send(NewRemark);

        if (errs != null && errs.Count == 0)
        {
            return RedirectToPage("/CodeRequests/Edit", new { id = NewRemark.CodeRequestId }).WithSuccess("Code Request Remark added");
        }

        foreach (var error in errs!)
        {
            ModelState.AddModelError(string.Empty, error);
        }

        await InitSelectListItems();

        return Page();
    }

    public async Task InitSelectListItems()
    {
        UserListVM? usrs = await _mediator.Send(new GetAppUsersQuery());
        AllStakeHolders = new SelectList(usrs.Users, "UserId", "DisplayName");
    }
}
