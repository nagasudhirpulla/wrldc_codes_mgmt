using Application.CodeRequests.Queries.GetCodeRequestsBetweenDates;
using Application.Common.Interfaces;
using Application.Users;
using Core.Entities;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.CodeRequests;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMediator _mediator;
    private readonly UserManager<ApplicationUser> _userManager;

    public IList<CodeRequestDTO> ReqList { get; set; } = new List<CodeRequestDTO>();

    public IndexModel(ILogger<IndexModel> logger, IMediator mediator, ICurrentUserService currentUserService, UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _mediator = mediator;
        _currentUserService = currentUserService;
        _userManager = userManager;
    }

    [BindProperty]
    public GetCodeRequestsBetweenDatesQuery Query { get; set; } = new() { StartDate = DateTime.Now.Date, EndDate = DateTime.Now.Date };
    public async Task OnGetAsync()
    {
        await PopulateReqListAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var validator = new GetCodeRequestsBetweenDatesQueryValidator();
        var validationResult = validator.Validate(Query);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, null);
            return Page();
        }

        await PopulateReqListAsync();
        return Page();
    }

    private async Task PopulateReqListAsync()
    {
        List<CodeRequestDTO> codeReqs = await _mediator.Send(Query);

        string? usrId = _currentUserService.UserId;
        ApplicationUser curUsr = await _userManager.FindByIdAsync(usrId);
        var isUsrAdminOrRldc = (await _userManager.GetRolesAsync(curUsr))
                                .Any(x => new List<string>() { SecurityConstants.AdminRoleString, SecurityConstants.RldcRoleString }.Contains(x));
        if (isUsrAdminOrRldc)
        {
            // get all code requests if the user is admin
            ReqList = codeReqs;
        }
        else
        {
            // filter the requests concerened with the logged in user
            // criteria is that the logged in user is either concerened stakeholder or requester
            ReqList = codeReqs.Where(x => (x.RequesterId == usrId) ||
                            x.ConcernedStakeholders.Select(x => x.Id).Contains(usrId))
                            .ToList();
        }
    }
}
