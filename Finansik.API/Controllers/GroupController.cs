using AutoMapper;
using Finansik.API.Models;
using Finansik.Domain.UseCases.CreateCategory;
using Finansik.Domain.UseCases.CreateGroup;
using Finansik.Domain.UseCases.GetGroups;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Finansik.API.Controllers;

[ApiController]
[Route("group")]
public class GroupController(IMediator mediator) : ControllerBase
{
    [HttpGet(Name = nameof(GetGroups))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Group[]))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    public async Task<IActionResult> GetGroups(
        [FromServices] IMapper mapper,
        CancellationToken cancellationToken)
    {
        var groups = await mediator.Send(new GetGroupsQuery(), cancellationToken);
        
        return Ok(groups.Select(mapper.Map<Group>));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Group))]
    public async Task<IActionResult> CreateGroup(
        [FromBody] CreateGroup request,
        [FromServices] IMapper mapper,
        CancellationToken cancellationToken)
    {
        var addedGroup = await mediator.Send(
            new CreateGroupCommand(request.Name, request.Icon), 
            cancellationToken);
        
        return CreatedAtRoute(nameof(GetGroups), mapper.Map<Group>(addedGroup));
    }
    
    [HttpPost("{groupId:guid}/category")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status410Gone)]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Category))]
    public async Task<IActionResult> CreateCategory(
        [FromRoute] Guid groupId,
        [FromBody] CreateCategory request,
        [FromServices] IMapper mapper,
        CancellationToken cancellationToken)
    {
        var command = new CreateCategoryCommand(groupId, request.Name, request.Icon);
        var category = await mediator.Send(command, cancellationToken);

        return CreatedAtRoute(nameof(CategoryController.GetCategories), mapper.Map<Category>(category));
    }
}