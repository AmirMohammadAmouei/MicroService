using Microsoft.AspNetCore.Mvc;

namespace Coupon.EndPoint.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
