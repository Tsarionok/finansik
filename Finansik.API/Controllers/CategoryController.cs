using Finansik.API.Models;
using Finansik.Domain.Exceptions;
using Finansik.Domain.UseCases.CreateCategory;
using Finansik.Domain.UseCases.GetCategories;
using Finansik.Domain.UseCases.RenameCategory;
using Microsoft.AspNetCore.Mvc;
using ValidationException = FluentValidation.ValidationException;

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
        var categories = await useCase.Execute(groupId, cancellationToken);
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
        var updatedCategory = await useCase.Execute(categoryId, name, cancellationToken);

        return Ok(new Category
        {
            Id = updatedCategory.Id,
            Icon = updatedCategory.Icon,
            Name = updatedCategory.Name
        });
    }
}