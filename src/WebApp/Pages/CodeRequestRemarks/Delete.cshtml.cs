using Application.CodeRequestRemarks.Commands.DeleteCodeRequestRemarks;
using Application.CodeRequestRemarks.Queries.GetRawCodeRequestRemark;
using Application.Users;
using Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Extensions;

namespace WebApp.Pages.CodeRequestRemarks;

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
    public CodeRequestRemark? CodeRequestRemark { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        CodeRequestRemark = await _mediator.Send(new GetRawCodeRequestRemarkQuery(id));
        if (CodeRequestRemark == null)
        {
            return NotFound();
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (CodeRequestRemark == null)
        {
            return NotFound();
        }

        CodeRequestRemark = await _mediator.Send(new GetRawCodeRequestRemarkQuery(CodeRequestRemark.Id));
        if (CodeRequestRemark == null)
        {
            return NotFound();
        }

        List<string> errs = await _mediator.Send(new DeleteCodeRequestRemarksCommand() { Id = CodeRequestRemark.Id });

        if (errs.Count == 0)
        {
            _logger.LogInformation("Code Request Remarks deleted successfully");
            return RedirectToPage("/CodeRequests/Edit", new { id = CodeRequestRemark.CodeRequestId }).WithSuccess("Code Request Remarks deletion done");
        }

        foreach (var error in errs)
        {
            ModelState.AddModelError(string.Empty, error);
        }

        return Page();
    }
}
