
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
               
            };

            return categoryVM;
        }

       
        
    }
}
