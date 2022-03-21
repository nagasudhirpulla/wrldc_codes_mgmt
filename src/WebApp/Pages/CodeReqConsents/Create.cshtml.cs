using Application.CodeRequestConsents.Commands.CreateCodeReqConsent;
using Application.Users.Queries.GetAppUsers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Extensions;

namespace WebApp.Pages.CodeReqConsents;

public class CreateModel : PageModel
{
    private readonly ILogger<CreateModel> _logger;
    private readonly IMediator _mediator;

    [BindProperty]
    public CreateCodeReqConsentCommand NewConsent { get; set; }

    public SelectList? ConsentLists { get; set; }

    public CreateModel(ILogger<CreateModel> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }
    public async Task<IActionResult> OnGetAsync(int id)
    {
        await InitSelectListItems();
        NewConsent = new() { CodeRequestId = id };
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (NewConsent == null)
        {
            return NotFound();
        }
        List<string> errs = await _mediator.Send(NewConsent);

        if (errs != null && errs.Count == 0)
        {
            return RedirectToPage("/CodeRequests/Edit", new { id = NewConsent.CodeRequestId }).WithSuccess("Code Request Consent added");
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
        ConsentLists = new SelectList(usrs.Users, "UserId", "DisplayName");
    }
}
