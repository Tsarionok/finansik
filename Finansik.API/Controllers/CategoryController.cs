using Finansik.API.Models;
using Finansik.Domain.UseCases.DeleteCategory;
using Finansik.Domain.UseCases.GetCategories;
using Finansik.Domain.UseCases.RenameCategory;
using Microsoft.AspNetCore.Mvc;

namespace Finansik.API.Controllers;

[ApiController]
[Route("category")]
public class CategoryController : ControllerBase
{
    [HttpGet(Name = nameof(GetCategories))]
    public async Task<IActionResult> GetCategories(
        Guid groupId,
        [FromServices] IGetCategoriesByGroupIdUseCase useCase,
        CancellationToken cancellationToken)
    {
        var categories = await useCase.ExecuteAsync(
            command: new GetCategoriesByGroupIdCommand(groupId), 
            cancellationToken);
        return Ok(categories.Select(c => new Category
        {
            Id = c.Id,
            Name = c.Name,
            Icon = c.Icon
        }));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateCategoryName(
        Guid categoryId,
        string name,
        [FromServices] IRenameCategoryUseCase useCase,
        CancellationToken cancellationToken)
    {
        var updatedCategory = await useCase.ExecuteAsync(
            command: new RenameCategoryCommand(categoryId, name), 
            cancellationToken);

        return Ok(new Category
        {
            Id = updatedCategory.Id,
            Icon = updatedCategory.Icon,
            Name = updatedCategory.Name
        });
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteCategory(
        [FromBody] DeleteCategory category,
        [FromServices] IDeleteCategoryUseCase useCase,
        CancellationToken cancellationToken)
    {
        var deletedCategory = await useCase.ExecuteAsync(
            new DeleteCategoryCommand(category.CategoryId), cancellationToken);
        
        return Ok(new Category
        {
            Id = deletedCategory.Id,
            Name = deletedCategory.Name,
            Icon = deletedCategory.Icon
        });
    }
}