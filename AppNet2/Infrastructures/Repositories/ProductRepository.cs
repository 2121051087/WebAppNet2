using AppNet2.Models;
using Microsoft.EntityFrameworkCore;
using WebAppNet2.Models.DTO;
using WebAppNet2.Models.Entities.Catalog;

namespace WebAppNet2.Infrastructures.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DbNet2Context _context;

        public ProductRepository(DbNet2Context context)
        {
            _context = context;
        }
        public Task<Products> AddProduct(ProductsVM model)
        {
            throw new NotImplementedException();
        }

        public Task<Products> DeleteProduct(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Products> GetProductById(Guid? id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Products>> GetProducts()
        {
          var products =  await _context.Products.ToListAsync();
            return products;
        }

        public Task<Products> UpdateProduct(Guid? id, Products products)
        {
            throw new NotImplementedException();
        }
    }
}
