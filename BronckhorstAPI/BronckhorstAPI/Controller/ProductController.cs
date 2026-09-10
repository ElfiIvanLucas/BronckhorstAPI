using BronckhorstAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BronckhorstAPI.Controller;

[Route("api/v1/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService ?? throw new ArgumentNullException(nameof(productService));
    }

    [HttpGet]
    [Route("GetProducts")]
    public async Task<IActionResult?> GetProducts()
    {
        try
        {
            var result = await _productService.GetAllProductsAsync();

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

    [HttpGet]
    [Route("GetProductById/{id}")]
    public async Task<IActionResult?> GetProductById(int id)
    {
        try
        {
            var result = await _productService.GetProductByIdAsync(id);

            return Ok(result);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }
        catch (Exception exception)
        {
            return StatusCode(500, exception.Message);
        }
    }
}
