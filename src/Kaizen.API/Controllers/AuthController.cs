using Kaizen.API.Models;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace Kaizen.API.Controllers;

/// <summary>
/// Used relevant parts Authentication implementation from<see href="https://github.com/dotnet/aspnetcore/blob/main/src/Identity/Core/src/IdentityApiEndpointRouteBuilderExtensions.cs">
/// original ASP.NET Identity source code</see>. Username and password bearer token authentication only.
/// </summary>
[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    [HttpPost("login")]
    public async Task<Results<Ok<AccessTokenResponse>, EmptyHttpResult, ProblemHttpResult>> Login(
        [FromBody] LoginRequest login,
        [FromServices] IServiceProvider sp)
    {
        var signInManager = sp.GetRequiredService<SignInManager<KaizenUser>>();

        signInManager.AuthenticationScheme = IdentityConstants.BearerScheme;

        var result = await signInManager.PasswordSignInAsync(login.Email, login.Password, false, false);

        if (!result.Succeeded)
            return TypedResults.Problem(result.ToString(), statusCode: StatusCodes.Status401Unauthorized);

        // The signInManager already produced the response in the form of a bearer token.
        return TypedResults.Empty;
    }
}