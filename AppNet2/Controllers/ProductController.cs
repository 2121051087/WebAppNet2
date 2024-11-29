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
            var product = await _unitOfWork.ProductRepository.GetProducts();
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
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            await _unitOfWork.ProductRepository.AddProduct(model);
            await _unitOfWork.SaveChangeAsync();
            return RedirectToAction("Index");
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> Delete(Guid? id)
        {
            var productVM = await _unitOfWork.ProductRepository.GetProductById(id);
            return View(productVM);
        }


        [HttpPost("{id}")]

        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _unitOfWork.ProductRepository.DeleteProduct(id);
                await _unitOfWork.SaveChangeAsync();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
           
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categories = await _unitOfWork.CategoriesRepository.GetCategories();
            ViewBag.Categories = categories.Select(c => new SelectListItem
            {
                Value = c.CategoryID.ToString(),
                Text = c.CategoryName
            }).ToList();

            var colors = await _unitOfWork.ProductRepository.GetColors();
            ViewBag.Colors = colors.Select(c => new { Value = c.ColorID.ToString(), Text = c.ColorName }).ToList();

            var sizes = await _unitOfWork.ProductRepository.GetSizes();
            ViewBag.Sizes = sizes.Select(s => new { Value = s.SizeID.ToString(), Text = s.SizeName }).ToList();

            var productVM = await _unitOfWork.ProductRepository.GetProductById(id);

            if (productVM == null)
            {
                return NotFound();
            }

            // Nếu colorSizesDTO là null, khởi tạo nó như một danh sách trống
            if (productVM.colorSizesDTO == null)
            {
                productVM.colorSizesDTO = new List<ColorSizesDTO>();
            }

            return View(productVM);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Edit(Guid? id, ProductsVM model)
        {
            if (id == null || model == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _unitOfWork.ProductRepository.UpdateProduct(id, model);
                await _unitOfWork.SaveChangeAsync();
                return RedirectToAction("Index");
            }

            // Re-populate ViewBag for dropdowns in case of error
            var categories = await _unitOfWork.CategoriesRepository.GetCategories();
            ViewBag.Categories = categories.Select(c => new SelectListItem
            {
                Value = c.CategoryID.ToString(),
                Text = c.CategoryName
            }).ToList();

            var colors = await _unitOfWork.ProductRepository.GetColors();
            ViewBag.Colors = colors.Select(c => new { Value = c.ColorID.ToString(), Text = c.ColorName }).ToList();

            var sizes = await _unitOfWork.ProductRepository.GetSizes();
            ViewBag.Sizes = sizes.Select(s => new { Value = s.SizeID.ToString(), Text = s.SizeName }).ToList();

            return View(model);
        }

    }
}
