using Kaizen.API.Models;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace Kaizen.API.Controllers;

/// <summary>
/// Taken from <see href="https://github.com/dotnet/aspnetcore/blob/main/src/Identity/Core/src/IdentityApiEndpointRouteBuilderExtensions.cs">original ASP.NET Identity source code</see>.
/// Only using required parts, with username and password authentication only.
/// </summary>
[ApiController]
[Route("[controller]")]
public class UserController
{
    [HttpPost("/login")]
    public async Task<Results<Ok<AccessTokenResponse>, EmptyHttpResult, ProblemHttpResult>> Login(
        [FromBody] LoginRequest login,
        [FromQuery] bool? useCookies,
        [FromQuery] bool? useSessionCookies,
        [FromServices] IServiceProvider sp)
    {
        var signInManager = sp.GetRequiredService<SignInManager<KaizenUser>>();

        var useCookieScheme = useCookies == true || useSessionCookies == true;
        var isPersistent = useCookies == true || useSessionCookies != true;
        
        signInManager.AuthenticationScheme =
            useCookieScheme ? IdentityConstants.ApplicationScheme : IdentityConstants.BearerScheme;

        var result =
            await signInManager.PasswordSignInAsync(login.Email, login.Password, isPersistent, lockoutOnFailure: false);

        if (!result.Succeeded)
            return TypedResults.Problem(result.ToString(), statusCode: StatusCodes.Status401Unauthorized);

        // The signInManager already produced the needed response in the form of a cookie or bearer token.
        return TypedResults.Empty;
    }
}