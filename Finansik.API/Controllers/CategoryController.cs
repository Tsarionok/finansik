using Finansik.API.Models;
using Finansik.Domain.Exceptions;
using Finansik.Domain.UseCases.RenameCategory;
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

    [HttpPut]
    public async Task<IActionResult> UpdateCategoryName(
        Guid categoryId,
        string name,
        [FromServices] IRenameCategoryUseCase useCase,
        CancellationToken cancellationToken)
    {
        try
        {
            var updatedCategory = await useCase.Execute(categoryId, name, cancellationToken);

            return Ok(new Category
            {
                Id = updatedCategory.Id,
                Icon = updatedCategory.Icon,
                Name = updatedCategory.Name
            });
        }
        catch (CategoryNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}