using Finansik.API.Controllers.Rest;
using Finansik.API.Models;
using Finansik.Domain.UseCases.GetGroups;
using Microsoft.AspNetCore.Mvc;

namespace Finansik.API.Controllers;

[ApiController]
[Route(Resources.Group)]
public class GroupController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Group[]))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    public async Task<IActionResult> GetGroups(
        [FromServices] IGetGroupsUseCase useCase,
        CancellationToken cancellationToken)
    {
        var groups = (await useCase.Execute(cancellationToken)).Select(g => new Group
        {
            Id = g.Id,
            Name = g.Name,
            Icon = g.Icon
        });
        return Ok(groups);
    }
}