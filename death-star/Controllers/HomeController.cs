using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DeathStar.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index([FromServices]IOptionsSnapshot<StarWarsConfig> config)
        {
            return Ok(config.Value);
        }
    }
}