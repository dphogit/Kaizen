using Kaizen.API.Models;
using Microsoft.AspNetCore.Identity;

namespace Kaizen.API.Middleware;

/// <summary>
/// Middleware to attach the current authenticated user to the <see cref="HttpContext" /> items dictionary.
/// If the request is not authenticated, then proceed without attaching.
/// </summary>
public class CurrentUserMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, UserManager<KaizenUser> userManager)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var user = await userManager.GetUserAsync(context.User);

            if (user is null)
            {
                throw new InvalidOperationException("Could not find user for authenticated HTTP context");
            }

            context.SetCurrentUser(user);
        }
        
        await next(context);
    }
}