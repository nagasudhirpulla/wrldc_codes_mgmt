using Application.CodeRequests.Commands.CreateOutageRevivalCodeRequest;
using Application.Common.Interfaces;
using Application.ReportingData.Queries.GetLatestUnrevivedOutages;
using Core.ReportingData;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.OutageRequests;

public class CreateOutageRevivalReqModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;
    public List<ReportingUnrevivedOutage> UnrevOtgs { get; set; } = new();

    [BindProperty]
    public CreateOutageRevivalCodeRequestCommand? Outage { get; set; }

    public CreateOutageRevivalReqModel(ILogger<IndexModel> logger, IMediator mediator, ICurrentUserService currentUserService)
    {
        _logger = logger;
        _mediator = mediator;
        _currentUserService = currentUserService;
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
        //TODO complete this
        await SetUnrevivedOutages();
        return Page();
    }

    private async Task SetUnrevivedOutages()
    {
        // get all latest unrevived outages
        UnrevOtgs = await _mediator.Send(new GetLatestUnrevivedOutagesQuery());
    }
}
