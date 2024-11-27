using AppNet2.Models;
using Microsoft.EntityFrameworkCore;
using WebAppNet2.Models.DTO;
using WebAppNet2.Models.Entities.Catalog;

namespace WebAppNet2.Infrastructures.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DbNet2Context _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductRepository(DbNet2Context context ,IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<Products> AddProduct(ProductsVM model)
        {

            string imagePath = string.Empty;

            if (model.ImagePath != null)
            {
                // Xử lý lưu file
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                string uniqueFileName = $"{Guid.NewGuid()}_{model.ImagePath.FileName}";
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Tạo thư mục nếu chưa tồn tại
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImagePath.CopyToAsync(fileStream);
                }

                // Lưu đường dẫn ảnh
                imagePath = $"/images/{uniqueFileName}";
            }

            // Tạo đối tượng Products
            var product = new Products
            {
                ProductName = model.ProductName,
                ProductDescription = model.ProductDescription,
                Price = model.Price,
                ImageProduct = imagePath, // Gán đường dẫn ảnh vào ImageProduct
                CategoryID = model.CategoryID,
                CreatedAt = DateTime.UtcNow.AddHours(7),
                UpdatedAt = DateTime.UtcNow.AddHours(7),
            };

            // Lưu vào cơ sở dữ liệu
            await _context.Products.AddAsync(product);

            foreach(var colorSize in model.colorSizesDTO)
            {

               var colorSizeEntity = new ColorSizes
                {
                    ColorID = colorSize.ColorID,
                    SizeID = colorSize.SizeID,
                    ProductID = product.ProductID,
                    Quantity = colorSize.Quantity
                };
                await _context.ColorSizes.AddAsync(colorSizeEntity);
            }
        
            return product;
        } 

        public Task<Products> DeleteProduct(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<ProductsVM> GetProductById(Guid? id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return null;
            }

            var productVM = new ProductsVM
            {
               
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                Price = product.Price,
                CurrentImagePath = product.ImageProduct,
                CategoryID = product.CategoryID,
               
            };
            return productVM;
        }

        public async Task<List<ProductsVM>> GetProducts()
        {
        
            var product = await _context.Products.Include(p => p.Category).ToListAsync();

            var colorSizes = await _context.ColorSizes
                .Include(cs => cs.Color)
                .Include(cs => cs.Sizes)
                .ToListAsync();

            var productVM = new List<ProductsVM>();

            foreach (var item in product)
            {
                var productColorSizes = colorSizes.Where(cs => cs.ProductID == item.ProductID)
                                                   .Select(cs => new ColorSidesDTO
                                                   {
                                                       ColorID = cs.ColorID,
                                                       SizeID = cs.SizeID,
                                                       Quantity = cs.Quantity,  
                                                       ColorName= cs.Color.ColorName,
                                                       SizeName = cs.Sizes.SizeName
                                                   }).ToList();
                var productVMItem = new ProductsVM
                {
                    ProductID = item.ProductID,
                    ProductName = item.ProductName,
                    ProductDescription = item.ProductDescription,
                    Price = item.Price,
                     
                    CurrentImagePath = item.ImageProduct,
                    UpdatedAt = item.UpdatedAt,
                    colorSizesDTO =productColorSizes,
                    CategoryName    = item.Category.CategoryName
                };

                productVM.Add(productVMItem);

            }
            return productVM;
        }

        public async Task<List<Colors>> GetColors()
        {
            var colors = await _context.Colors.ToListAsync();
            return colors;
        }

        public async Task<List<Sizes>> GetSizes() 
        {
            var sizes = await _context.Sizes.ToListAsync();
            return sizes;
        }

        public Task<Products> UpdateProduct(Guid? id, Products products)
        {
            throw new NotImplementedException();
        }
    }
}
