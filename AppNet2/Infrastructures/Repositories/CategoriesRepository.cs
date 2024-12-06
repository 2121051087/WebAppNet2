  
using AppNet2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppNet2.Models.DTO;
using WebAppNet2.Models.Entities.Catalog;

namespace WebAppNet2.Infrastructures.Repositories
{
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly DbNet2Context _context;

        public CategoriesRepository( DbNet2Context context)
        {
            _context = context;
        }
        public async Task<Categories> AddCategory(CategoriesVM model)
        {
            var category = new Categories
            {
                CategoryID = Guid.NewGuid(),
                CategoryName = model.CategoryName
            };
          
            await _context.Categories.AddAsync(category);
            return category;
        }

        public async Task<Categories> DeleteCategory(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return null;
            }
             _context.Categories.Remove(category);
            return category;
       
        }

        public async Task<IEnumerable<Categories>> GetCategories()
        {
            var result = await _context.Categories.ToListAsync();
            return result;
        }

        public  async Task<List<CategoriesVM>> GetAllCategories() 
        { 
            var result = await _context.Categories.Select(c => new CategoriesVM
            {
                CategoryID = c.CategoryID,
                CategoryName = c.CategoryName,
                CountNumberProductByCategory = _context.Products.Where(p => p.CategoryID == c.CategoryID).Count()
            }).ToListAsync();

            return result;
        }

        public async Task<Categories> UpdateCategory(Guid? id,CategoriesVM model)
        {
           var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return null;
            }
            category.CategoryName = model.CategoryName;

             _context.Categories.Update(category);
            return category;
          
           
        }

        public async Task<CategoriesVM> GetCategoryById (Guid? id)
        {

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return null;
            }

            var categoryVM = new CategoriesVM
            {
                CategoryName = category.CategoryName,
                CategoryID = category.CategoryID,
                CountNumberProductByCategory = _context.Products.Where(p => p.CategoryID == category.CategoryID).Count()
               
            };

            return categoryVM;
        }

        public async Task<List<CategoriesVM>> GetProductsByCategory()
        {
            var result = await _context.Categories
                .Include(c => c.Products)
                  .ThenInclude(p => p.ColorSizes)
                    .ThenInclude(cs => cs.Color)
                .Include(c => c.Products)
                  .ThenInclude(p => p.ColorSizes)
                    .ThenInclude(cs => cs.Sizes)
                .Where(c => c.Products.Any())
                .ToListAsync();


            var categoriesVM = result.Select(c => new CategoriesVM
            {
                CategoryID = c.CategoryID,
                CategoryName = c.CategoryName,
                Products = c.Products.Select(p => new ProductsVM
                {
                    ProductID = p.ProductID,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    ProductDescription= p.ProductDescription,
                
                     CurrentImagePath = p.ImageProduct,
                     colorSizesDTO = p.ColorSizes.Select(cs => new ColorSizesDTO
                    {
                         ColorSizesID = cs.ColorSizesID,
                         ColorID = cs.ColorID,
                         SizeID = cs.SizeID,
                         ColorHexCode = cs.Color.ColorHexCode,
                         ColorName = cs.Color.ColorName,
                         SizeName = cs.Sizes.SizeName,
                        Quantity = cs.Quantity
                    }).ToList(),
                }).ToList()
            }).ToList();

            return categoriesVM;
        }
       
      
        public async Task<int> CountNumberProductByCategory(Guid? id)
        {
            var count = await _context.Products.Where(p => p.CategoryID == id).CountAsync();

            return count;
        }
        
    }
}
