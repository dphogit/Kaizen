using Kaizen.API.Models;

namespace Kaizen.API.Middleware;

public static class CurrentUserMiddlewareExtensions
{
    private const string CurrentUserKey = "CurrentUser";
    
    public static IApplicationBuilder UseCurrentUserMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CurrentUserMiddleware>();
    }

    public static HttpContext SetCurrentUser(this HttpContext context, KaizenUser user)
    {
        context.Items[CurrentUserKey] = user;
        return context;
    }

    public static KaizenUser GetCurrentUser(this HttpContext context)
    {
        if (context.Items.TryGetValue(CurrentUserKey, out var user) && user is KaizenUser kaizenUser)
        {
            return kaizenUser;
        }

        throw new InvalidOperationException("Could not get current user from HTTP context.");
    }
}