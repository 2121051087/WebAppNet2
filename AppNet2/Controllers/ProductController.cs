using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebAppNet2.Infrastructures.UnitOfWork;
using WebAppNet2.Models.DTO;

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
          var product =  await _unitOfWork.ProductRepository.GetProducts();
            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _unitOfWork.CategoriesRepository.GetCategories();
            ViewBag.Categories = categories.Select(c => new SelectListItem
            {
                Value = c.CategoryID.ToString(),
                Text = c.CategoryName
            }).ToList();


            var colors = await _unitOfWork.ProductRepository.GetColors();
            ViewBag.Colors = colors.Select(c => new
            {
                Value = c.ColorID.ToString(),
                Text = c.ColorName
            }).ToList();

            var sizes = await _unitOfWork.ProductRepository.GetSizes();
            ViewBag.Sizes = sizes.Select(s => new
            {
                Value = s.SizeID.ToString(),
                Text = s.SizeName
            }).ToList();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductsVM model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            await _unitOfWork.ProductRepository.AddProduct(model);
            await _unitOfWork.SaveChangeAsync();
            return RedirectToAction("Index");
        }

        //[HttpGet]

        //public Task<IActionResult> Delete()
        //{
        //    return View();
        //}
    }
}
