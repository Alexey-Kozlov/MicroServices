using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MainAPI.Controllers
{
    public class HomeController : Controller
    {
        //[Route("home/login")]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        //[Route("home/logout")]
        public IActionResult Logout()
        {
            return SignOut("Cookies", "oidc");
        }

        [HttpGet]
        //[Route("home/index")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
