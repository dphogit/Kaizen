using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Kaizen.API.Controllers;

[ApiController]
public class ErrorController(ILogger<ErrorController> logger) : ControllerBase
{
    [Route("/error")]
    public IActionResult Error()
    {
        var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

        if (exceptionHandlerPathFeature is null)
        {
            return NotFound();
        }

        var path = exceptionHandlerPathFeature.Path;
        var exception = exceptionHandlerPathFeature.Error;
        
        logger.LogError(exception, "Unhandled exception occured at {Path}", path);

        return Problem(
            detail: "An internal server error occured.",
            statusCode: StatusCodes.Status500InternalServerError,
            instance: path);
    }
}