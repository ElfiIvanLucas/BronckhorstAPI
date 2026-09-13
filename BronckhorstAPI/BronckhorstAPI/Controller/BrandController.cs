using BronckhorstAPI.Application.DTO;
using BronckhorstAPI.Application.Services.Interfaces;
using BronckhorstAPI.Constants;
using Microsoft.AspNetCore.Mvc;

namespace BronckhorstAPI.Controller;

[Route("api/" + ApIVersions.V1 + "/[controller]")]
[ApiController]
public class BrandController : ApiControllerBase
{
    private readonly IBrandService _brandService;

    public BrandController(IBrandService brandService)
    {
        _brandService = brandService ?? throw new ArgumentNullException(nameof(brandService));
    }

    [HttpGet]
    [Route("GetProducts")]
    [ProducesResponseType(typeof(List<BrandDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult?> GetProducts()
    {
        try
        {
            var result = await _brandService.GetAllBrandsAsync();

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
}
