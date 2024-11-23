using WebAppNet2.Models.DTO;
using WebAppNet2.Models.Entities.Catalog;

namespace WebAppNet2.Infrastructures.Repositories
{
    public interface IProductRepository
    {
        public Task<Products> AddProduct(ProductsVM model);
        public Task<Products> DeleteProduct(Guid id);
        public Task<List<Products>> GetProducts();
        public Task<Products> UpdateProduct(Guid? id, Products products);
        public Task<Products> GetProductById(Guid? id);
    }
}
