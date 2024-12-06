using AppNet2.Models;
using Microsoft.AspNetCore.Mvc;
using WebAppNet2.Infrastructures;

namespace WebAppNet2.Areas.Admin.Controllers
{
    [Area("Admin")]
    [RoleAuthorize(AppRole.Admin)]
    public class DashBoardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
