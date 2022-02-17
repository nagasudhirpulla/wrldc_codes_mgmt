using Application.CodeRequests.Commands.CreateApprovedOutageCodeRequest;
using Application.Common.Interfaces;
using Application.ReportingData.Queries.GetApprovedOutagesForDate;
using Application.UserStakeHolders.Queries.GetUserStakeholders;
using Core.Entities;
using Core.ReportingData;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
