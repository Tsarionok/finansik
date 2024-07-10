using AutoMapper;
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
        [FromServices] IMapper mapper,
        CancellationToken cancellationToken)
    {
        var categories = await useCase.ExecuteAsync(
            command: new GetCategoriesByGroupIdCommand(groupId), 
            cancellationToken);
        
        return Ok(categories.Select(mapper.Map<IEnumerable<Category>>));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateCategoryName(
        Guid categoryId,
        string name,
        [FromServices] IRenameCategoryUseCase useCase,
        [FromServices] IMapper mapper,
        CancellationToken cancellationToken)
    {
        var updatedCategory = await useCase.ExecuteAsync(
            command: new RenameCategoryCommand(categoryId, name), 
            cancellationToken);

        return Ok(mapper.Map<Category>(updatedCategory));
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteCategory(
        [FromBody] DeleteCategory category,
        [FromServices] IDeleteCategoryUseCase useCase,
        [FromServices] IMapper mapper,
        CancellationToken cancellationToken)
    {
        var deletedCategory = await useCase.ExecuteAsync(
            new DeleteCategoryCommand(category.CategoryId), cancellationToken);
        
        return Ok(mapper.Map<Category>(deletedCategory));
    }
}