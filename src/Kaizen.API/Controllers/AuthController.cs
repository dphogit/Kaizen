using Kaizen.API.Contracts;
using Kaizen.API.Models;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace Kaizen.API.Controllers;

/// <summary>
/// Used relevant parts of the authentication implementation from<see href="https://github.com/dotnet/aspnetcore/blob/main/src/Identity/Core/src/IdentityApiEndpointRouteBuilderExtensions.cs">
/// the original ASP.NET Identity source code</see>.
/// </summary>
[ApiController]
[Route("[controller]")]
public class AuthController(
    ILogger<AuthController> logger,
    SignInManager<KaizenUser> signInManager,
    UserManager<KaizenUser> userManager) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<Results<Ok<AccessTokenResponse>, EmptyHttpResult, ProblemHttpResult>> Login(
        [FromBody] LoginRequest login,
        [FromQuery] bool useCookies = false,
        [FromQuery] bool useSessionCookies = false)
    {
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

    [HttpGet]
    public async Task<Results<Ok<KaizenUserDto>, InternalServerError>> Me()
    {
        var user = await GetUserFromContext();
        
        var roles = await userManager.GetRolesAsync(user);
        
        var mock = new KaizenUserDto
        {
            Id = user.Id,
            Email = user.Email!,
            Roles = roles.ToArray(),
        };
        
        logger.LogInformation("{Email} retrieved own user data", user.Email);

        return TypedResults.Ok(mock);
    }

    private async Task<KaizenUser> GetUserFromContext()
    {
        var user = await userManager.GetUserAsync(User);

        if (user is null)
        {
            throw new InvalidOperationException("Could not find user for given authenticated context");
        }
        
        return user;
    }
}