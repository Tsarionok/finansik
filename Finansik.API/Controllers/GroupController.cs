using Finansik.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Finansik.API.Controllers;

[ApiController]
[Route("[controller]")]
public class GroupController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string[]))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    public async Task<IActionResult> GetGroups(
        [FromServices] FinansikDbContext context,
        CancellationToken cancellationToken)
    { 
        var groupNames = await context.Groups.Select(g => g.Name).ToListAsync(cancellationToken);
        return Ok(groupNames);
    }
}