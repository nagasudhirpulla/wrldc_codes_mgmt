using Application.CodeRequests.Commands.CreateApprovedOutageCodeRequest;
using Application.Common.Interfaces;
using Application.ReportingData.Queries.GetApprovedOutagesForDate;
using Application.UserStakeHolders.Queries.GetUserStakeholders;
using Core.Entities;
using Core.ReportingData;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Extensions;

namespace WebApp.Pages.OutageRequests;

[Authorize]
public class CreateApprovedOutageReqModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;
    public List<ReportingOutageRequest> OReqs { get; set; } = new();

    [BindProperty]
    public CreateApprovedOutageCodeRequestCommand? NewReq { get; set; }

    public CreateApprovedOutageReqModel(ILogger<IndexModel> logger, IMediator mediator, ICurrentUserService currentUserService)
    {
        _logger = logger;
        _mediator = mediator;
        _currentUserService = currentUserService;
    }

    public async Task OnGetAsync()
    {
        await SetOutageRequests();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        ValidationResult validationCheck = new CreateApprovedOutageCodeRequestCommandValidator().Validate(NewReq!);
        validationCheck.AddToModelState(ModelState, nameof(NewReq));


        if (!ModelState.IsValid)
        {
            await SetOutageRequests();
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
        await SetOutageRequests();
        return Page();
    }

    private async Task SetOutageRequests()
    {
        // check if user is authorized
        string? curUsrId = _currentUserService.UserId;

        // get the user requester Ids
        List<UserStakeholder>? userStakeHolders = await _mediator.Send(new GetUserStakeholdersQuery() { UsrId = curUsrId });
        List<int>? requesterIds = userStakeHolders.Select(x => x.StakeHolderId).ToList();

        // get all approved outage requests
        DateTime targetDt = DateTime.Now;
        OReqs = await _mediator.Send(new GetApprovedOutagesForDateQuery() { ReqDt = targetDt });

        // filter the outage requests to have only the desired requester Ids
        OReqs = OReqs.Where(x => requesterIds.Contains(x.RequesterId)).ToList();
    }
}
