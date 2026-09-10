using BronckhorstAPI.DTO;
using BronckhorstAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BronckhorstAPI.Controller;

[Route("api/v1/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
    }

    [HttpGet]
    [Route("GetCategories")]
    [ProducesResponseType(typeof(List<CategoryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult?> GetCategories()
    {
        try
        {
            var result = await _categoryService.GetAllCategoriesAsync();

            if (result.Count != 0)
            {
                return Ok(result);
            }

            return NotFound();
        }
        catch (Exception exception)
        {
            return StatusCode(500, exception.Message);
        }
    }
}
