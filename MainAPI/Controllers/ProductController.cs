using Microsoft.AspNetCore.Mvc;

namespace MainAPI.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
