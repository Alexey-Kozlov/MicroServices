using MainAPI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Principal;

namespace MainAPI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IIdentityService _identityService;
        public HomeController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [Route("home/login")]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            var user = User.Identity;

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("home/logout")]
        public IActionResult Logout()
        {
            return SignOut("Cookies", "oidc");
        }
    }
}
