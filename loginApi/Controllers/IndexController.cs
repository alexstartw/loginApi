using Microsoft.AspNetCore.Mvc;

namespace loginApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class IndexController : Controller
{
    [HttpGet("{filename}")]
    public IActionResult  Get( string filename)
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(),
            "wwwroot", filename);
        var stream = new FileStream(filePath, FileMode.Open);
        return File(stream, "text/html");
    }
    
}