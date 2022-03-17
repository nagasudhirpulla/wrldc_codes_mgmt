using Application.CodeRequestRemarks.Queries.GetRawCodeRequestRemark;
using Application.CodeRequestRemarks.Commands.EditCodeRequestRemarks;
using Application.CodeRequests.Queries.GetCodeRequestById;
using Application.Users;
using Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AutoMapper;
using WebApp.Extensions;

namespace WebApp.Pages.CodeRequestRemarks;


public class EditModel : PageModel
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public EditModel(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [BindProperty]
    public EditCodeRequestRemarksCommand? CodeRequestRemark { get; set; }

    public string? RLDCRemarks { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        CodeRequestRemark? remarksReq = await _mediator.Send(new GetRawCodeRequestRemarkQuery(id));
        if (remarksReq == null)
        {
            return NotFound();
        }
        CodeRequestRemark = _mapper.Map<EditCodeRequestRemarksCommand>(remarksReq);
        RLDCRemarks = remarksReq!.RldcRemarks;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (CodeRequestRemark == null)
        {
            return NotFound();
        }
        List<string> errs = await _mediator.Send(CodeRequestRemark);

        if (errs != null && errs.Count == 0)
        {
            return RedirectToPage("./Index").WithSuccess("Code Request Remark edited");
        }

        CodeRequestRemark? remarksReq = await _mediator.Send(new GetRawCodeRequestRemarkQuery(CodeRequestRemark.Id));
        if (remarksReq == null)
        {
            return NotFound();
        }
        RLDCRemarks = remarksReq!.RldcRemarks;

        foreach (var error in errs!)
        {
            ModelState.AddModelError(string.Empty, error);
        }

        return Page();
    }
}
