using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Kaizen.API.Controllers;

[ApiController]
[Route("[controller]")]
[TypeFilter<DevOnlyFilterAttribute>]
public class DevController : ControllerBase
{
    [HttpGet]
    public IActionResult Ping()
    {
        return Ok("Development Route");
    }
    
    [HttpGet("throw")]
    public IActionResult ThrowException()
    {
        throw new Exception("Development Exception");
    }
}

public class DevOnlyFilterAttribute(IWebHostEnvironment environment) : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (environment.IsDevelopment())
        {
            base.OnActionExecuting(context);
            return;
        }

        context.Result = new NotFoundResult();
    }
}