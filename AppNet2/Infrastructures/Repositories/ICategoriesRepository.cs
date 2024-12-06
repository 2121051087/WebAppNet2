using WebAppNet2.Models.DTO;
using WebAppNet2.Models.Entities.Catalog;

namespace WebAppNet2.Infrastructures.Repositories
{
    public interface ICategoriesRepository
    {
        public Task<IEnumerable<Categories>> GetCategories();

        public Task<Categories> AddCategory(CategoriesVM model);

        public Task<Categories> UpdateCategory(Guid? id,CategoriesVM model);

        public Task<Categories> DeleteCategory(Guid id);

        public Task<CategoriesVM> GetCategoryById(Guid? id);

        public Task<List<CategoriesVM>> GetProductsByCategory();

        public Task<int> CountNumberProductByCategory(Guid? id);

        public Task<List<CategoriesVM>> GetAllCategories();



    }
}
