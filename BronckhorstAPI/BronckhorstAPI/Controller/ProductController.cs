using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BronckhorstAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        [HttpGet]
        [Route("GetStatus")]
        public IActionResult GetStatus()
        {
            return Ok();
        }
    }
}
