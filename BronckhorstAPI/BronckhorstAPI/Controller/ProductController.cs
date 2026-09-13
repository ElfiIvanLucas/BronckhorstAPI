using BronckhorstAPI.Application.DTO;
using BronckhorstAPI.Application.Filters;
using BronckhorstAPI.Application.Pagination;
using BronckhorstAPI.Application.Services.Interfaces;
using BronckhorstAPI.Constants;
using Microsoft.AspNetCore.Mvc;

namespace BronckhorstAPI.Controller;

[Route("api/" + ApIVersions.V1 + "/[controller]")]
[ApiController]
public class ProductController : ApiControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService ?? throw new ArgumentNullException(nameof(productService));
    }

    [HttpGet]
    [Route("GetProducts")]
    [ProducesResponseType(typeof(List<ProductDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult?> GetProducts()
    {
        try
        {
            var result = await _productService.GetAllProductsAsync();

            if (result.Count > 0)
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

    [HttpPost]
    [Route("GetProductsByFilters")]
    [ProducesResponseType(typeof(PageResult<ProductDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult?> GetProductsByFiltersAsync([FromBody] ProductFilters productFilters)
    {
        try
        {
            var result = await _productService.GetProductsByFiltersAsync(productFilters);

            return Ok(result);
        }
        catch (Exception exception)
        {
            return HandleException(exception);
        }
    }

    [HttpGet]
    [Route("GetProductById/{productId:int}")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult?> GetProductById(int productId)
    {
        try
        {
            ValidateId(productId, nameof(productId));

            var result = await _productService.GetProductByIdAsync(productId);

            return Ok(result);
        }
        catch (Exception exception)
        {
            return HandleException(exception);
        }
    }
}
