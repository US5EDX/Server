using Server.Services.Options.ContextOptions.RequestContext;
using System.Security.Claims;

namespace Server.Middleware;

public class RequestContextMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context, IRequestContext requestContext)
    {
        requestContext.IpAddress = context.Connection.RemoteIpAddress?.ToString();

        if (context.User.Identity?.IsAuthenticated == true)
        {
            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier) ??
                              context.User.FindFirst("sub");

            requestContext.UserId = userIdClaim?.Value;
        }

        await next(context);
    }
}
