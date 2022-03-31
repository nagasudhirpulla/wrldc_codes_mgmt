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
using Application.ReportingData.Queries.GetOwners;
using Application.UserElementOwners.Queries.GetUserElementOwners;

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


    public MultiSelectList ElementOwnerOptions { get; set; }

    [BindProperty]
    public int[] SelectedElementOwnerIds { get; set; }

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

        List<ReportingOwner> allElementOwners = await _mediator.Send(new GetOwnersQuery());
        List<UserElementOwner> existingElementOwners = await _mediator.Send(new GetUserElementOwnersQuery() { UsrId = user.Id });
        SelectedElementOwnerIds = existingElementOwners.Select(i => i.OwnerId).ToArray();

        InitSelectListItems(allStakeholders, existingStakeholders, allElementOwners, existingElementOwners);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        List<ReportingStakeholder> allStakeholders = await _mediator.Send(new GetStakeholdersQuery());
        List<UserStakeholder> existingStakeholders = await _mediator.Send(new GetUserStakeholdersQuery() { UsrId = UpUser.Id });

        // make sure that existing stakeholders are present in all stakeholders
        var allStakeholderIds = allStakeholders.Select(x => x.Id);
        foreach (var eS in existingStakeholders)
        {
            if (!allStakeholderIds.Contains(eS.StakeHolderId))
            {
                allStakeholders.Add(new ReportingStakeholder(eS.StakeHolderId, eS.StakeHolderName ?? eS.StakeHolderId.ToString()));
            }
        }

        List<ReportingOwner> allElementOwners = await _mediator.Send(new GetOwnersQuery());
        List<UserElementOwner> existingElementOwners = await _mediator.Send(new GetUserElementOwnersQuery() { UsrId = UpUser.Id });

        // make sure that existing element-owners are present in all element-owners
        var allElementOwnerIds = allElementOwners.Select(x => x.Id);
        foreach (var eO in existingElementOwners)
        {
            if (!allElementOwnerIds.Contains(eO.OwnerId))
            {
                allElementOwners.Add(new ReportingOwner(eO.OwnerId, eO.OwnerName ?? eO.OwnerId.ToString()));
            }
        }

        InitSelectListItems(allStakeholders, existingStakeholders, allElementOwners, existingElementOwners);

        ValidationResult validationCheck = new EditUserCommandValidator().Validate(UpUser);
        validationCheck.AddToModelState(ModelState, nameof(UpUser));

        if (!ModelState.IsValid)
        {
            return Page();
        }

        // set the updated stakeholders in the command
        UpUser.Stakeholders = allStakeholders.Where(x => SelectedStakeHolderIds.Contains(x.Id)).ToList();

        // set the updated element owners in the command
        UpUser.ElementOwners = allElementOwners.Where(x => SelectedElementOwnerIds.Contains(x.Id)).ToList();

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

    public void InitSelectListItems(List<ReportingStakeholder> allStakeholders, List<UserStakeholder> existingStakeholders, List<ReportingOwner> allElementOwners, List<UserElementOwner> existingElementOwners)
    {
        URoles = new SelectList(SecurityConstants.GetRoles());

        List<ReportingStakeholder> stakeholderOptions = new(allStakeholders);
        // make sure existing stakeholders are present in the select list
        foreach (var eS in existingStakeholders)
        {
            if (!stakeholderOptions.Any(x => x.Id.Equals(eS.StakeHolderId)))
            {
                stakeholderOptions.Add(new ReportingStakeholder(eS.StakeHolderId, eS.StakeHolderName ?? eS.StakeHolderId.ToString()));
            }
        }
        StakeholderOptions = new MultiSelectList(stakeholderOptions, nameof(ReportingStakeholder.Id), nameof(ReportingStakeholder.Username), existingStakeholders.Select(x => x.StakeHolderId));

        List<ReportingOwner> elementOwnerOptions = new(allElementOwners);
        // make sure existing element-owners are present in the select list
        foreach (var eO in existingElementOwners)
        {
            if (!elementOwnerOptions.Any(x => x.Id.Equals(eO.OwnerId)))
            {
                elementOwnerOptions.Add(new ReportingOwner(eO.OwnerId, eO.OwnerName ?? eO.OwnerId.ToString()));
            }
        }
        ElementOwnerOptions = new MultiSelectList(elementOwnerOptions, nameof(ReportingOwner.Id), nameof(ReportingOwner.Name), existingElementOwners.Select(x => x.OwnerId));
    }
}
