using Microsoft.AspNetCore.Mvc;

namespace WebAppNet2.Areas.Customer.Controllers
{
    public class HomeController : Controller
    {
        [Area("Customer")]
        
        public IActionResult Index()
        {
            return View();
        }
    }
}
