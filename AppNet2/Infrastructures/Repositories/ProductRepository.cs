using AppNet2.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
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

        public async Task<Products> DeleteProduct(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return null;
            }
            if (!string.IsNullOrEmpty(product.ImageProduct))
            {
                string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", Path.GetFileName(product.ImageProduct));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            _context.Products.Remove(product);
            return product;
        }

        public async Task<ProductsVM> GetProductById(Guid? id)
        {
            var product = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.ProductID == id);

            var productColorSize = await _context.ColorSizes
                .Where(cs => cs.ProductID == id)
                .Select(cs => new ColorSizesDTO
                {
                    ColorSizesID = cs.ColorSizesID,
                    ColorID = cs.ColorID,
                    SizeID = cs.SizeID,
                    Quantity = cs.Quantity,
                    ColorName = cs.Color.ColorName,
                    SizeName = cs.Sizes.SizeName
                }).ToListAsync();

            if (product == null || productColorSize == null)
            {
                return null;
            }

            var productVM = new ProductsVM
            {
                ProductID = product.ProductID,
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                Price = product.Price,
                CurrentImagePath = product.ImageProduct,
                CategoryID = product.CategoryID,
                CategoryName = product.Category.CategoryName,
                UpdatedAt = product.UpdatedAt,
                colorSizesDTO = productColorSize,
                //ColorSizesDTOJson = JsonSerializer.Serialize(productColorSize)
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
                                                   .Select(cs => new ColorSizesDTO
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

        public async Task<Products> UpdateProduct(Guid? id, ProductsVM model)
        {
            var product = await _context.Products.Include(p => p.ColorSizes).FirstOrDefaultAsync(p => p.ProductID == id);
            if (product == null)
            {
                return null;
            }

            // Update basic fields
            product.ProductName = model.ProductName;
            product.ProductDescription = model.ProductDescription;
            product.Price = model.Price;
            product.CategoryID = model.CategoryID;
            product.UpdatedAt = DateTime.UtcNow.AddHours(7);

            // Handle Image update if needed
            if (model.ImagePath != null)
            {
                if (!string.IsNullOrEmpty(product.ImageProduct))
                {
                    var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", product.ImageProduct);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                string uniqueFileName = $"{Guid.NewGuid()}_{model.ImagePath.FileName}";
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImagePath.CopyToAsync(fileStream);
                }

                product.ImageProduct = $"/images/{uniqueFileName}";
            }

            // Handle ColorSizes
            var currentColorSizes = await _context.ColorSizes
                .Where(cs => cs.ProductID == product.ProductID)
                .ToListAsync();

            var colorSizesIDsInDTO = model.colorSizesDTO
                .Where(cs => cs.ColorSizesID != Guid.Empty)
                .Select(cs => cs.ColorSizesID)
                .ToList();

            // Remove ColorSizes that are no longer in the model
            foreach (var colorSize in currentColorSizes)
            {
                if (!colorSizesIDsInDTO.Contains(colorSize.ColorSizesID))
                {
                    _context.ColorSizes.Remove(colorSize);
                }
            }

            // Add or update ColorSizes
            foreach (var colorSize in model.colorSizesDTO)
            {
                if (colorSize.ColorSizesID != Guid.Empty)
                {
                    // Update existing ColorSize
                    var existColorSize = await _context.ColorSizes
                        .FirstOrDefaultAsync(cs => cs.ColorSizesID == colorSize.ColorSizesID);

                    if (existColorSize != null)
                    {
                        existColorSize.ColorID = colorSize.ColorID;
                        existColorSize.SizeID = colorSize.SizeID;
                        existColorSize.Quantity = colorSize.Quantity;
                        _context.ColorSizes.Update(existColorSize);
                    }
                }
                else if (colorSize.ColorID != Guid.Empty && colorSize.SizeID != Guid.Empty)
                {
                    // Add new ColorSize
                    var newColorSize = new ColorSizes
                    {
                        ColorSizesID = Guid.NewGuid(),
                        ColorID = colorSize.ColorID,
                        SizeID = colorSize.SizeID,
                        ProductID = product.ProductID,
                        Quantity = colorSize.Quantity
                    };
                    await _context.ColorSizes.AddAsync(newColorSize);
                }
            }

            await _context.SaveChangesAsync();  // Save changes to the database

            return product;
        }



    }
}
