using Application.CodeRequests.Queries.GetCodeRequestById;
using Application.Users;
using Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.CodeRequests;

[Authorize(Roles = SecurityConstants.AdminRoleString + "," + SecurityConstants.RldcRoleString)]
public class EditModel : PageModel
{
    private readonly IMediator _mediator;
    public CodeRequest? CodeRequest { get; set; }

    public EditModel(IMediator mediator)
    {
        _mediator = mediator;
    }
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        CodeRequest = await _mediator.Send(new GetCodeRequestByIdQuery() { CodeReqId = id.Value });
        if (CodeRequest == null)
        {
            return NotFound();
        }
        
        return Page();
    }

}
