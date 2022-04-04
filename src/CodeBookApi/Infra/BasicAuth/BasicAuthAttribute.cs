using Microsoft.AspNetCore.Mvc;

namespace CodeBookApi.Infra.BasicAuth;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class BasicAuthAttribute : TypeFilterAttribute
{
    // https://github.com/dotnet-labs/ApiAuthDemo/blob/master/ApiAuthDemo/Infrastructure/BasicAuth/BasicAuthAttribute.cs
    public BasicAuthAttribute(string realm = @"Realm1") : base(typeof(BasicAuthFilter))
    {
        Arguments = new object[] { realm };
    }
}
