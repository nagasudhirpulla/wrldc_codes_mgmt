using Application.CodeRequests.Commands.CreateElementOutageCodeRequest;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Extensions;
using FluentValidation.Results;
using FluentValidation.AspNetCore;
using Application.ReportingData.Queries.GetElementTypes;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Pages.OutageRequests;

public class CreateElementOutageReqModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;

    public CreateElementOutageReqModel(ILogger<IndexModel> logger, IMediator mediator, ICurrentUserService currentUserService)
    {
        _logger = logger;
        _mediator = mediator;
        _currentUserService = currentUserService;
    }
    [BindProperty]
    public CreateElementOutageCodeRequestCommand? NewReq { get; set; }
    public SelectList ElementTypesOptions { get; set; }
    public async Task OnGet()
    {
        await InitSelectListItems();
        int a = 1;
    }
    public async Task<IActionResult> OnPostAsync()
    {
        ValidationResult validationCheck = new CreateElementOutageCodeRequestCommandValidator().Validate(NewReq!);
        validationCheck.AddToModelState(ModelState, nameof(NewReq));


        if (!ModelState.IsValid)
        {

            return Page();
        }

        List<string> errs = await _mediator.Send(NewReq!);
        if (errs.Count == 0)
        {
            _logger.LogInformation($"Created new approved outage code request");
            return RedirectToPage().WithSuccess($"Created new approved outage code request");
        }

        foreach (var error in errs)
        {
            ModelState.AddModelError(string.Empty, error);
        }

        return Page();
    }
    public async Task InitSelectListItems()
    {
        ElementTypesOptions = new SelectList(await _mediator.Send(new GetElementTypesQuery()), "Id", "Name");
    }
}
