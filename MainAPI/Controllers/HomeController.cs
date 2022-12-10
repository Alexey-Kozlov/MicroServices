using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MainAPI.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {

        }

        [Route("home/login")]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            var user = User.Identity;

            return Ok("444");
        }
    }
}
