using Application.CodeRequestConsents.Commands.DeleteCodeReqConsent;
using Application.CodeRequestConsents.Queries.GetRawCodeReqConsent;
using Application.Users;
using Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Extensions;

namespace WebApp.Pages.CodeReqConsents;

[Authorize(Roles = SecurityConstants.AdminRoleString + "," + SecurityConstants.RldcRoleString)]
public class DeleteModel : PageModel
{
    private readonly ILogger<DeleteModel> _logger;
    private readonly IMediator _mediator;

    public DeleteModel(ILogger<DeleteModel> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [BindProperty]
    public CodeRequestConsent? CodeReqConsent { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        CodeReqConsent = await _mediator.Send(new GetRawCodeReqConsentQuery(id));
        if (CodeReqConsent == null)
        {
            return NotFound();
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (CodeReqConsent == null)
        {
            return NotFound();
        }

        CodeReqConsent = await _mediator.Send(new GetRawCodeReqConsentQuery(CodeReqConsent.Id));
        if (CodeReqConsent == null)
        {
            return NotFound();
        }

        List<string> errs = await _mediator.Send(new DeleteCodeReqConsentCommand() { Id = CodeReqConsent.Id });

        if (errs.Count == 0)
        {
            _logger.LogInformation("Code Request Consent deleted successfully");
            return RedirectToPage("/CodeRequests/Edit", new { id = CodeReqConsent.CodeRequestId }).WithSuccess("Code Request Stakeholder deletion done");
        }

        foreach (var error in errs)
        {
            ModelState.AddModelError(string.Empty, error);
        }

        return Page();
    }
}
