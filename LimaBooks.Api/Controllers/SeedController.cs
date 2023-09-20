using LimaBooks.Service.BookService;
using LimaBooks.Service.SeedService;
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
    }
}
