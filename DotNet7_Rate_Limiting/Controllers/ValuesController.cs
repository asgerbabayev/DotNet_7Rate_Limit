using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace DotNet7_Rate_Limiting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableRateLimiting("Sliding")]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Data listed");
        }
    }
}
