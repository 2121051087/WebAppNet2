using WebAppNet2.Models.DTO;
using WebAppNet2.Models.Entities.Catalog;

namespace WebAppNet2.Infrastructures.Repositories
{
    public interface IProductRepository
    {
        public Task<Products> AddProduct(ProductsVM model);
        public Task<Products> DeleteProduct(Guid id);
        public Task<List<ProductsVM>> GetProducts();
        public Task<Products> UpdateProduct(Guid? id, ProductsVM model);
        public Task<ProductsVM> GetProductById(Guid? id);

        public Task<List<Colors>> GetColors();

        public Task<List<Sizes>> GetSizes();
    }
}
