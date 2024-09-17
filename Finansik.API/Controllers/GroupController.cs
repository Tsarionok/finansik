using AutoMapper;
using Finansik.API.Models;
using Finansik.Domain.UseCases.AddMemberToGroup;
using Finansik.Domain.UseCases.CreateCategory;
using Finansik.Domain.UseCases.CreateGroup;
using Finansik.Domain.UseCases.GetGroupById;
using Finansik.Domain.UseCases.GetGroups;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Finansik.API.Controllers;

[ApiController]
[Route("groups")]
public sealed class GroupController(IMediator mediator) : ControllerBase
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

    [HttpGet("{groupId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Group))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    public async Task<IActionResult> GetGroup(
        [FromRoute] Guid groupId,
        [FromServices] IMapper mapper,
        CancellationToken cancellationToken)
    {
        var group = await mediator.Send(new GetGroupByIdQuery(groupId), cancellationToken);
        
        return Ok(mapper.Map<Group>(group));
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

    [HttpPost]
    public async Task<IActionResult> AddMember(
        [FromBody] AddMember request,
        CancellationToken cancellationToken)
    {
        var command = new AddMemberToGroupCommand(request.GroupId, request.UserId);
        var groupInfo = await mediator.Send(command, cancellationToken);

        return Ok(groupInfo);
    }
}