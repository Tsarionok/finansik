using Finansik.API.Models;
using Finansik.Domain.UseCases.CreateCategory;
using Finansik.Domain.UseCases.CreateGroup;
using Finansik.Domain.UseCases.GetGroups;
using Microsoft.AspNetCore.Mvc;

namespace Finansik.API.Controllers;

[ApiController]
[Route("group")]
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

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Group))]
    public async Task<IActionResult> AddGroup(
        // TODO: replace "name" and "icon" from request parameters to body
        string name, string icon,
        [FromServices] ICreateGroupUseCase useCase, 
        CancellationToken cancellationToken)
    {
        var addedGroup = await useCase.Execute(name, icon, cancellationToken);
        return Ok(new Group
        {
            Id = addedGroup.Id,
            Name = addedGroup.Name,
            Icon = addedGroup.Icon
        });
    }
    
    [HttpPost("{groupId:guid}/category")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status410Gone)]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Category))]
    public async Task<IActionResult> CreateCategory(
        [FromRoute] Guid groupId,
        [FromBody] CreateCategory request,
        [FromServices] ICreateCategoryUseCase useCase,
        CancellationToken cancellationToken)
    {
        var command = new CreateCategoryCommand(groupId, request.Name, request.Icon);
        var category = await useCase.Execute(command, cancellationToken);

        return CreatedAtRoute(nameof(CategoryController.GetCategories), new Category
        {
            Id = category.Id,
            Name = category.Name,
            Icon = category.Icon
        });
    }
}