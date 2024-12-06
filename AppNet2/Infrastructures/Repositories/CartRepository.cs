using AppNet2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebAppNet2.Models.DTO;
using WebAppNet2.Models.Entities.Carts;

namespace WebAppNet2.Infrastructures.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly DbNet2Context _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClaimsPrincipal? _user;

        public CartRepository(DbNet2Context context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _user = httpContextAccessor.HttpContext?.User;
        }

       

        public async Task AddItemToCartAsync(CartItemDTO model)
        {
            // Lấy userID từ Claims (Nếu người dùng đã đăng nhập)
            var userID = _user?.FindFirstValue(ClaimTypes.NameIdentifier);

            // Nếu userID không có trong Claims, lấy từ session
            if (string.IsNullOrEmpty(userID))
            {
                userID = _httpContextAccessor.HttpContext?.Session.GetString("UserID");
            }

            if (string.IsNullOrEmpty(userID))
            {
                throw new Exception("Không thể xác định userID.");
            }

            // Lấy hoặc tạo giỏ hàng
            var cart = await GetOrCreateCartAsync(userID);

            // Kiểm tra nếu sản phẩm đã tồn tại trong giỏ hàng
            var existingCartItem = await _context.CartItem
                                                 .FirstOrDefaultAsync(ci => ci.CartID == cart.CartID &&
                                                                            ci.ProductID == model.ProductID &&
                                                                            ci.ColorSizesID == model.ColorSizeID);

            if (existingCartItem != null)
            {
                // Nếu đã tồn tại, cập nhật số lượng
                existingCartItem.Quantity += model.Quantity;
            }
            else
            {
                // Nếu chưa tồn tại, thêm mới vào giỏ hàng
                var newCartItem = new CartItem
                {
                    CartID = cart.CartID,
                    Cart_itemID = Guid.NewGuid(),
                    ProductID = model.ProductID,
                    ColorSizesID = model.ColorSizeID,
                    Quantity = model.Quantity
                };

                await _context.CartItem.AddAsync(newCartItem);
            }

           
        }

        
        public async Task<Carts> GetOrCreateCartAsync(string userID)
        {
            var existingCart = await _context.Carts
                                            .Include(c => c.cartItems)
                                            .ThenInclude(ci => ci.Products)
                                            .FirstOrDefaultAsync(c => c.UserID == userID);

            if(existingCart != null)
            {
                return existingCart;
            }

            var newCart = new Carts
            {
                UserID = userID,
                CreatedAt = DateTime.UtcNow.AddHours(7),
                UpdatedAt = DateTime.UtcNow.AddHours(7)
            };
            
            await _context.Carts.AddAsync(newCart);
            await _context.SaveChangesAsync();

            return newCart;

                                           
        }

  

        public async Task<List<CartItemDTO>> ListItemCartAsync()
        {
          
            var userId = _httpContextAccessor.HttpContext.Session.GetString("UserID");

     
            var items = await _context.CartItem
                .Include(c => c.ColorSizes).ThenInclude(cs => cs.Color)
                .Include(c => c.ColorSizes).ThenInclude(cs => cs.Sizes)
                .Include(c => c.Products)
                .Where(c => c.Carts.UserID == userId)
                .ToListAsync();

            
            var cartItemDTOs = items.Select(item => new CartItemDTO
            {
                ProductID = item.ProductID,
                ColorSizeID = item.ColorSizesID,
                Quantity = item.Quantity,



                Cart_itemID = item.Cart_itemID,
                ProductName = item.Products.ProductName,
                Price = item.Products.Price,
                ColorName = item.ColorSizes.Color.ColorName,
                SizeName = item.ColorSizes.Sizes.SizeName,
                Image = item.Products.ImageProduct,

                
            }).ToList();

            return cartItemDTOs;
        }


        public async Task UpdateQuantity (CartItemDTO model)
        {
          var existCartItem = await _context.CartItem.FirstOrDefaultAsync(ci => ci.Cart_itemID == model.Cart_itemID);

            if (existCartItem == null)
            {
                throw new Exception("CartItem not found.");
            }

            existCartItem.Quantity = model.Quantity;

           _context.CartItem.Update(existCartItem);

        }

        public async Task RemoveItemFromCartAsync(Guid id)
        {
            var existCartItem = await _context.CartItem.FirstOrDefaultAsync(ci => ci.Cart_itemID == id);

            if (existCartItem == null)
            {
                throw new Exception("CartItem not found.");
            }
             _context.CartItem.Remove(existCartItem);
        }


        public async Task<int> GetCartItemCountAsync()
        {
            var userId = _httpContextAccessor.HttpContext.Session.GetString("UserID");

            var result = await _context.CartItem
                .Where(c => c.Carts.UserID == userId)
                .CountAsync();  // Sử dụng CountAsync để thực hiện truy vấn bất đồng bộ
            return result;
        }

    }
}
