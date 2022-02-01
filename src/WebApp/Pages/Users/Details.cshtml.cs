using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Application.Common.Interfaces;
using Application.Users.Queries.GetAppUsers;
using Application.Users.Queries.GetUserById;
using Application.Users.Queries.IsUsrSelfOrAdmin;

namespace WebApp.Pages.Users;

public class DetailsModel : PageModel
{

    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;

    public DetailsModel(IMediator mediator, ICurrentUserService currentUserService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
    }
    public UserDTO CUser { get; set; }
    public string RequestId { get; set; }

    public async Task<IActionResult> OnGetAsync(string id)
    {
        RequestId = id;
        // check if user is authorized
        string curUsrId = _currentUserService.UserId;

        // show details only for self or admin
        bool isUsrSelfOrAdmin = await _mediator.Send(new IsUsrSelfOrAdminQuery() { UsrId = id ?? curUsrId });
        if (!isUsrSelfOrAdmin)
        {
            return Unauthorized();
        }

        CUser = await _mediator.Send(new GetUserByIdQuery() { Id = id ?? curUsrId });
        if (CUser == null)
        {
            return NotFound();
        }

        return Page();
    }
}
