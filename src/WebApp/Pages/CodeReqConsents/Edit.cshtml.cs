using Application.CodeRequestConsents.Commands.EditCodeReqConsent;
using Application.CodeRequestConsents.Queries.GetRawCodeReqConsent;
using Application.CodeRequests.Queries.GetCodeRequestById;
using Application.Users;
using AutoMapper;
using Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Extensions;

namespace WebApp.Pages.CodeReqConsents;


public class EditModel : PageModel
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    [BindProperty]
    public EditCodeReqConsentCommand? ConsentRequest { get; set; }
    public string? RLDCRemarks { get; set; }

    public EditModel(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }
    public async Task<IActionResult> OnGetAsync(int id)
    {
        CodeRequestConsent? consentReq = await _mediator.Send(new GetRawCodeReqConsentQuery(id));
        if (consentReq == null)
        {
            return NotFound();
        }
        RLDCRemarks = consentReq!.RldcRemarks;
        ConsentRequest = _mapper.Map<EditCodeReqConsentCommand>(consentReq);
        if (ConsentRequest == null)
        {
            return NotFound();
        }
        
        return Page();
    }
    public async Task<IActionResult> OnPostAsync()
    {
        if (ConsentRequest == null)
        {
            return NotFound();
        }
        List<string> errs = await _mediator.Send(ConsentRequest);

        if (errs != null && errs.Count == 0)
        {
            return RedirectToPage("./Index", new { }).WithSuccess("Code Request consent added");
        }
        CodeRequestConsent? consentReq = await _mediator.Send(new GetRawCodeReqConsentQuery(ConsentRequest.Id));
        if (consentReq == null)
        {
            return NotFound();
        }
        RLDCRemarks = consentReq!.RldcRemarks;
        foreach (var error in errs!)
        {
            ModelState.AddModelError(string.Empty, error);
        }

        return Page();
    }
   

}
