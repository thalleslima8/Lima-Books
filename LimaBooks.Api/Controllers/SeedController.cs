using LimaBooks.Service.BookService;
using LimaBooks.Service.SeedService;
using LimaBooks.Shared.Filters;
using Microsoft.AspNetCore.Mvc;

namespace LimaBooks.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController : Controller
    {
        private readonly ISeedService _seedService;

        public SeedController(ISeedService seedService)
        {
            _seedService = seedService;
        }

        [HttpGet]
        public virtual async Task<IActionResult> Get()
        {
            try
            {
                var result = await _seedService.Seed();
                return Ok(result);
            }
            catch (Exception exception)
            {
                return BadRequest(new { Error = $"{exception.Message}" });
            }
        }
    }
}
