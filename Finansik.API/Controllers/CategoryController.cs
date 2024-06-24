using Microsoft.AspNetCore.Mvc;

namespace Finansik.API.Controllers;

[ApiController]
[Route("category")]
public class CategoryController : ControllerBase
{
    [HttpGet]
    public IActionResult GetCategories()
    {
        return NotFound();
    }
}