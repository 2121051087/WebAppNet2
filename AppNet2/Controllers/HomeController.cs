using AppNet2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebAppNet2.Infrastructures.UnitOfWork;
using WebAppNet2.Models.DTO;

namespace AppNet2.Controllers
{

    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger ,IUnitOfWork unitOfWork )
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public IActionResult Index()
        {
            //var isUserLoggedIn = HttpContext.Session.GetString("UserId")  != null;

            //ViewBag.IsUserLoggedIn = isUserLoggedIn;
            var model = new HomeViewModel
            {
                ListProductsByCatgory = _unitOfWork.CategoriesRepository.GetProductsByCategory().Result,
                CategoriesVM = _unitOfWork.CategoriesRepository.GetAllCategories().Result,

            };
                
                
                
            return View(model);
        }
        
        public IActionResult ProductDetails(Guid id)
        {
            var product = _unitOfWork.ProductRepository.GetProductById(id).Result;

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult SizeGuideTable()
        {
            return View();
        }


        public IActionResult StoreSystem()
        {
           return View();
        }
    }
}
