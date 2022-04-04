using Application.CodeRequests.Commands.EditCodeRequest;
using Application.CodeRequests.Queries.GetCodeRequestById;
using Application.Users;
using AutoMapper;
using Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Extensions;

namespace WebApp.Pages.CodeRequests;

[Authorize(Roles = SecurityConstants.AdminRoleString + "," + SecurityConstants.RldcRoleString)]
public class EditModel : PageModel
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    
    public CodeRequest? CodeRequest { get; set; }
    [BindProperty]
    public EditElementOutageCodeRequestCommand NewReq { get; set; }
public EditModel(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
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
        NewReq = _mapper.Map<EditElementOutageCodeRequestCommand>(CodeRequest);
        if (NewReq == null)
        {
            return NotFound();
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (NewReq == null)
        {
            return NotFound();
        }
        List<string> errs = await _mediator.Send(NewReq);

        if (errs != null && errs.Count == 0)
        {
            return RedirectToPage("./Index", new { }).WithSuccess("Code Request edited");
        }
        // TODO perform editing and redirect to index page

        CodeRequest = await _mediator.Send(new GetCodeRequestByIdQuery() { CodeReqId = NewReq.Id });
        if (CodeRequest == null)
        {
            return NotFound();
        }
        foreach (var error in errs!)
        {
            ModelState.AddModelError(string.Empty, error);
        }
        return Page();
    }

}
