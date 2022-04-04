using CodeBookApi.Infra.BasicAuth;
using Microsoft.AspNetCore.Mvc;

namespace CodeBookApi.Controllers;


[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly ILogger<AccountController> _logger;

    public AccountController(ILogger<AccountController> logger)
    {
        _logger = logger;
    }

    [HttpGet("basic-logout")]
    [BasicAuth]
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult BasicAuthLogout()
    {
        _logger.LogInformation("basic auth logout");
        // NOTE: there's no good way to log out basic authentication. This method is a hack.
        HttpContext.Response.Headers["WWW-Authenticate"] = "Basic realm=\"Realm1\"";
        return new UnauthorizedResult();
    }
}