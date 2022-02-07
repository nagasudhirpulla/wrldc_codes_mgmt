using AutoMapper;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Application.Users;
using Application.Users.Commands.EditUser;
using Application.Users.Queries.GetRawUserById;
using Core.Entities;
using WebApp.Extensions;
using Application.ReportingData.Queries.GetStakeholders;
using Application.UserStakeHolders.Queries.GetUserStakeholders;
using Core.ReportingData;

namespace WebApp.Pages.Users;

[Authorize(Roles = SecurityConstants.AdminRoleString)]
public class EditModel : PageModel
{
    private readonly ILogger<EditModel> _logger;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public EditModel(ILogger<EditModel> logger, IMediator mediator, IMapper mapper)
    {
        _logger = logger;
        _mediator = mediator;
        _mapper = mapper;
    }

    [BindProperty]
    public EditUserCommand UpUser { get; set; }

    public SelectList URoles { get; set; }


    public MultiSelectList StakeholderOptions { get; set; }

    [BindProperty]
    public int[] SelectedStakeHolderIds { get; set; }

    public async Task<IActionResult> OnGetAsync(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        ApplicationUser user = await _mediator.Send(new GetRawUserByIdQuery() { Id = id });
        if (user == null)
        {
            return NotFound();
        }

        UpUser = _mapper.Map<EditUserCommand>(user);

        List<ReportingStakeholder> allStakeholders = await _mediator.Send(new GetStakeholdersQuery());
        List<UserStakeholder> existingStakeholders = await _mediator.Send(new GetUserStakeholdersQuery() { UsrId = user.Id });
        SelectedStakeHolderIds = existingStakeholders.Select(i => i.StakeHolderId).ToArray();

        InitSelectListItems(allStakeholders, existingStakeholders);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        List<ReportingStakeholder> allStakeholders = await _mediator.Send(new GetStakeholdersQuery());
        List<UserStakeholder> existingStakeholders = await _mediator.Send(new GetUserStakeholdersQuery() { UsrId = UpUser.Id });
        InitSelectListItems(allStakeholders, existingStakeholders);

        ValidationResult validationCheck = new EditUserCommandValidator().Validate(UpUser);
        validationCheck.AddToModelState(ModelState, nameof(UpUser));

        if (!ModelState.IsValid)
        {
            return Page();
        }

        // set the updated stakeholders in the command
        UpUser.Stakeholders = allStakeholders.Where(x => SelectedStakeHolderIds.Contains(x.Id)).ToList();

        List<string> errors = await _mediator.Send(UpUser);

        foreach (var error in errors)
        {
            ModelState.AddModelError(string.Empty, error);
        }

        // check if we have any errors and redirect if successful
        if (errors.Count == 0)
        {
            _logger.LogInformation("User edit operation successful");
            return RedirectToPage($"./{nameof(Index)}").WithSuccess("User Editing done");
        }

        return Page();
    }

    public void InitSelectListItems(List<ReportingStakeholder> allStakeholders, List<UserStakeholder> existingStakeholders)
    {
        URoles = new SelectList(SecurityConstants.GetRoles());

        List<ReportingStakeholder> stakeholderOptions = new(allStakeholders);
        // make sure existing stakeholders are present in the select list
        foreach (var eS in existingStakeholders)
        {
            if (!stakeholderOptions.Any(x => x.Id.Equals(eS.StakeHolderId)))
            {
                stakeholderOptions.Add(new ReportingStakeholder(eS.StakeHolderId, eS.StakeHolderName ?? eS.Id.ToString()));
            }
        }
        StakeholderOptions = new MultiSelectList(stakeholderOptions, "Id", "Username", existingStakeholders.Select(x => x.StakeHolderId));
    }
}
