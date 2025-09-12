using Microsoft.AspNetCore.Mvc;

namespace Kaizen.API.Controllers;

[Route("/")]
public class HomeController : ControllerBase
{
    [HttpGet]
    public string Get()
    {
        return "Hello, World!";
    }
}