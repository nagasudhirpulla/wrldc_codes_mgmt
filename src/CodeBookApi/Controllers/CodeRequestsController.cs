using CodeBookApi.Infra.BasicAuth;
using Microsoft.AspNetCore.Mvc;

namespace CodeBookApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CodeRequestsController : ControllerBase
{
    [BasicAuth] // You can optionally provide a specific realm --> [BasicAuth("my-realm")]
    [HttpGet(Name = "GetCodes")]
    public async Task<List<string>> PostAsync()
    {
        return await Task.FromResult(new List<string>() { "abc", "def", "ghi" });
    }
}
