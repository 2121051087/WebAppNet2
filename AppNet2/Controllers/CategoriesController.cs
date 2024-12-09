using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppNet2.Infrastructures.UnitOfWork;
using WebAppNet2.Models.DTO;
using WebAppNet2.Models.Entities.Catalog;

namespace WebAppNet2.Controllers
{
    [Route("Admin/[Controller]/[Action]")]
    public class CategoriesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoriesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            var categoryVM = await _unitOfWork.CategoriesRepository.GetAllCategories();

            
            return View(categoryVM);
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] string searchString)
        {
            var categoryVM = await _unitOfWork.CategoriesRepository.GetAllCategories();

            // lọc theo searchString
            // If searchString is provided, filter categories by CategoryName
            if (!string.IsNullOrEmpty(searchString))
            {
                categoryVM = categoryVM
                             .Where(c => c.CategoryName != null && c.CategoryName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                             .ToList();
            }

            // Return the filtered list to the view (or JSON if you're working with an API)
            return View("Index", categoryVM);  // Or return Json(categoryVM) if this is an API
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CategoriesVM model)
        {

            if (ModelState.IsValid)
            {
                await _unitOfWork.CategoriesRepository.AddCategory(model);
                await _unitOfWork.SaveChangeAsync();
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Error");
            return View(model);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var category = await _unitOfWork.CategoriesRepository.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Edit(Guid? id,CategoriesVM model)
        {

            if (id == null)
            {
                return NotFound();
            }
           // ModelState.Remove("CategoryID");
            if (ModelState.IsValid)
            {
                try
                {
                    await _unitOfWork.CategoriesRepository.UpdateCategory(id, model);
                    await _unitOfWork.SaveChangeAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    var exist = _unitOfWork.CategoriesRepository.GetCategoryById(id);

                    if (exist == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(model);


        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Delete(Guid? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            var category = await _unitOfWork.CategoriesRepository.GetCategoryById(id);

            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        [HttpPost("{id}")]

        public async Task<IActionResult> Delete(Guid id)
        {

            await _unitOfWork.CategoriesRepository.DeleteCategory(id);
            await _unitOfWork.SaveChangeAsync();
            return RedirectToAction("Index");
        }


    }
}

