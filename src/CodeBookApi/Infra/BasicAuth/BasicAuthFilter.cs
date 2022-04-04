using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace CodeBookApi.Infra.BasicAuth;

public class BasicAuthFilter : IAuthorizationFilter
{
    // https://github.com/dotnet-labs/ApiAuthDemo/blob/master/ApiAuthDemo/Infrastructure/BasicAuth/BasicAuthFilter.cs
    private readonly string _realm;

    public BasicAuthFilter(string realm)
    {
        _realm = realm;
        if (string.IsNullOrWhiteSpace(_realm))
        {
            throw new ArgumentNullException(nameof(realm), @"Please provide a non-empty realm value.");
        }
    }
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        try
        {
            string authHeader = context.HttpContext.Request.Headers["Authorization"];
            if (authHeader != null)
            {
                var authHeaderValue = AuthenticationHeaderValue.Parse(authHeader);
                if (authHeaderValue.Scheme.Equals(AuthenticationSchemes.Basic.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    var apiKey = Encoding.UTF8
                                        .GetString(Convert.FromBase64String(authHeaderValue.Parameter ?? string.Empty));
                    if (IsAuthorized(context, apiKey))
                    {
                        return;
                    }
                }
            }

            ReturnUnauthorizedResult(context);
        }
        catch (FormatException)
        {
            ReturnUnauthorizedResult(context);
        }
    }

    public bool IsAuthorized(AuthorizationFilterContext context, string apiKey)
    {
        // check for API key from configuration
        string? correctApiKey = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>().GetValue<string>("ApiKey");
        if (string.IsNullOrWhiteSpace(correctApiKey))
        {
            return false;
        }
        if (correctApiKey == apiKey)
        {
            return true;
        }
        return false;
    }

    private void ReturnUnauthorizedResult(AuthorizationFilterContext context)
    {
        // Return 401 and a basic authentication challenge (causes browser to show login dialog)
        context.HttpContext.Response.Headers["WWW-Authenticate"] = $"Basic realm=\"{_realm}\"";
        context.Result = new UnauthorizedResult();
    }
}
