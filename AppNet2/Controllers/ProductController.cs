using Microsoft.AspNetCore.Mvc;
using WebAppNet2.Infrastructures.UnitOfWork;

namespace WebAppNet2.Controllers
{
    [Route("Admin/[Controller]/[Action]")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await _unitOfWork.ProductRepository.GetProducts();
            return View();
        }
    }
}
