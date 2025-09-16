using Kaizen.API.Models;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace Kaizen.API.Controllers;

/// <summary>
/// Used relevant parts Authentication implementation from<see href="https://github.com/dotnet/aspnetcore/blob/main/src/Identity/Core/src/IdentityApiEndpointRouteBuilderExtensions.cs">
/// original ASP.NET Identity source code</see>.
/// </summary>
[ApiController]
[Route("[controller]")]
public class AuthController(ILogger<AuthController> logger) : ControllerBase
{
    [HttpPost("login")]
    public async Task<Results<Ok<AccessTokenResponse>, EmptyHttpResult, ProblemHttpResult>> Login(
        [FromBody] LoginRequest login,
        [FromServices] IServiceProvider sp,
        [FromQuery] bool useCookies = false,
        [FromQuery] bool useSessionCookies = false)
    {
        var signInManager = sp.GetRequiredService<SignInManager<KaizenUser>>();

        var useCookieScheme = useCookies || useSessionCookies;
        var isPersistent = useCookies && !useSessionCookies;

        signInManager.AuthenticationScheme =
            useCookieScheme ? IdentityConstants.ApplicationScheme : IdentityConstants.BearerScheme;

        var result = await signInManager.PasswordSignInAsync(login.Email, login.Password, isPersistent, false);

        if (!result.Succeeded)
        {
            logger.LogInformation("{Email} login failed", login.Email);
            return TypedResults.Problem(result.ToString(), statusCode: StatusCodes.Status401Unauthorized);
        }

        // The signInManager already produced the response in the form of a bearer token.
        logger.LogInformation("{Email} login succeeded", login.Email);
        return TypedResults.Empty;
    }
}