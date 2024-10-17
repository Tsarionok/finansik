using AutoMapper;
using Finansik.API.Models;
using Finansik.Domain.UseCases.DeleteCategory;
using Finansik.Domain.UseCases.GetCategories;
using Finansik.Domain.UseCases.RenameCategory;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Finansik.API.Controllers;

[ApiController]
[Route("category")]
public sealed class CategoryController(IMediator mediator) : ControllerBase
{
    [HttpGet(Name = nameof(GetCategories))]
    public async Task<IActionResult> GetCategories(
        Guid groupId,
        [FromServices] IMapper mapper,
        CancellationToken cancellationToken)
    {
        var categories = await mediator.Send(
            new GetCategoriesByGroupIdQuery(groupId), 
            cancellationToken);
        
        return Ok(categories.Select(mapper.Map<Category>));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateCategoryName(
        Guid categoryId,
        string name,
        [FromServices] IMapper mapper,
        CancellationToken cancellationToken)
    {
        var updatedCategory = await mediator.Send(
            new RenameCategoryCommand(categoryId, name), 
            cancellationToken);

        return Ok(mapper.Map<Category>(updatedCategory));
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteCategory(
        [FromBody] DeleteCategory category,
        [FromServices] IMapper mapper,
        CancellationToken cancellationToken)
    {
        var deletedCategory = await mediator.Send(
            new DeleteCategoryCommand(category.CategoryId), cancellationToken);
        
        return Ok(mapper.Map<Category>(deletedCategory));
    }
}