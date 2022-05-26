using Microsoft.AspNetCore.Mvc;

namespace Moe.Afa.Utils.API.Controllers;

[ApiController]
[Route("hello")]
public class Hello : ControllerBase
{
    // GET
    [HttpGet]
    public IActionResult Index()
    {
        return Ok("Hello");
    }
}