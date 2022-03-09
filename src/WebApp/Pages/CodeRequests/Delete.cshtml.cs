using Application.CodeRequests.Commands.DeleteCodeRequest;
using Application.CodeRequests.Queries.GetRawCodeRequestById;
using Application.Users;
using Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Extensions;

namespace WebApp.Pages.CodeRequests;

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
    public CodeRequest? CodeRequest { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        CodeRequest = await _mediator.Send(new GetRawCodeRequestByIdQuery() { CodeReqId = id });

        if (CodeRequest == null)
        {
            return NotFound();
        }

        return Page();
    }

    // create post request handler for delete code request
    public async Task<IActionResult> OnPostAsync()
    {
        if (CodeRequest == null)
        {
            return NotFound();
        }

        List<string> errs = await _mediator.Send(new DeleteCodeRequestCommand() { Id = CodeRequest.Id });

        if (errs.Count == 0)
        {
            _logger.LogInformation("Code request with {id} deleted successfully", CodeRequest.Id);
            return RedirectToPage($"./{nameof(Index)}").WithSuccess("Code request deletion done");
        }

        foreach (var error in errs)
        {
            ModelState.AddModelError(string.Empty, error);
        }

        return Page();
    }

}
