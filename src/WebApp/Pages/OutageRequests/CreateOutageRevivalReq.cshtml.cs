using Application.CodeRequests.Commands.CreateOutageRevivalCodeRequest;
using Application.ReportingData.Queries.GetLatestUnrevivedOutages;
using Core.ReportingData;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Extensions;

namespace WebApp.Pages.OutageRequests;

public class CreateOutageRevivalReqModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IMediator _mediator;
    public List<ReportingOutage> UnrevOtgs { get; set; } = new();

    [BindProperty]
    public CreateOutageRevivalCodeRequestCommand? Outage { get; set; }

    public CreateOutageRevivalReqModel(ILogger<IndexModel> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }
    public async Task OnGetAsync()
    {
        await SetUnrevivedOutages();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        ValidationResult validationCheck = new CreateOutageRevivalCodeRequestCommandValidator().Validate(Outage!);
        validationCheck.AddToModelState(ModelState, nameof(Outage));


        if (!ModelState.IsValid)
        {
            await SetUnrevivedOutages();
            return Page();
        }
        List<string> errs = await _mediator.Send(Outage!);
        if (errs.Count == 0)
        {
            _logger.LogInformation($"Created new revival code request");
            return RedirectToPage().WithSuccess($"Created new revival code request");
        }

        foreach (var error in errs)
        {
            ModelState.AddModelError(string.Empty, error);
        }

        await SetUnrevivedOutages();
        return Page();
    }

    private async Task SetUnrevivedOutages()
    {
        // get all latest unrevived outages
        UnrevOtgs = await _mediator.Send(new GetLatestUnrevivedOutagesQuery());
    }
}
