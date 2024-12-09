using AppNet2.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebAppNet2.Models.DTO;
using WebAppNet2.Models.Entities.Orders;

namespace WebAppNet2.Infrastructures.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly DbNet2Context _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClaimsPrincipal? _user;
        private readonly ICartRepository _cartRepository;

        public OrdersRepository(DbNet2Context context, IHttpContextAccessor httpContextAccessor , ICartRepository cartRepository)
        {
            _context = context; 
            _httpContextAccessor = httpContextAccessor;
            _user = httpContextAccessor.HttpContext?.User;
            _cartRepository = cartRepository;
        }
        public async Task CreateOrderAsync(OrdersDTO model)
        {
            var userID = _httpContextAccessor.HttpContext?.Session.GetString("UserID");

            if (string.IsNullOrEmpty(userID))
            {
                throw new Exception("Không thể xác định userID.");
            }

            var order = new Orders
            {
                OrderID = Guid.NewGuid(),
                OrderDate = DateTime.UtcNow.AddHours(7),
                UserID = Guid.Parse(userID),
                FullName = model.fullName,
                Status = "Pending",
                Address = model.Address,
                City = model.City,
                District = model.District,
                Ward = model.Ward,
                PhoneNumber = model.PhoneNumber,

               
            };

            var exist = await _cartRepository.GetOrCreateCartAsync(userID);

            if(exist == null)
            {
                throw new Exception("Cart is empty");
            }

            var cartItems = await _context.CartItem.Include(ci => ci.Products)
                .Where(ci => ci.CartID == exist.CartID).ToListAsync();


            var orderDetails = new List<OrderDetails>();
            foreach(var item in cartItems)
            {
                var product =  await _context.Products.FirstOrDefaultAsync(p => p.ProductID == item.ProductID);

                if (product == null)
                {
                    throw new Exception("Product not found");
                }

                var colorSize = await _context.ColorSizes.FirstOrDefaultAsync(cs => cs.ColorSizesID == item.ColorSizesID);
                colorSize.Quantity -= item.Quantity;

                if (colorSize.Quantity < 0)
                {
                    throw new Exception("Sản phẩm đã hết hàng");
                }

                orderDetails.Add(new OrderDetails
                {
                    OrderDetailID = Guid.NewGuid(),
                    OrderID = order.OrderID,
                    ProductID = item.ProductID,
                    ColorSizesID = item.ColorSizesID,
                    Quantity = item.Quantity,
                    Price = product.Price

                });

            }

            order.OrderDetails = orderDetails;

            order.TotalAmount = orderDetails.Sum(od => od.Price * od.Quantity);

            await _context.Orders.AddAsync(order);
        }


    }
}
