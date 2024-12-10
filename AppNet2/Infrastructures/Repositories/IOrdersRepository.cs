using AppNet2.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebAppNet2.Models.DTO;
using WebAppNet2.Models.Entities.Orders;

namespace WebAppNet2.Infrastructures.Repositories
{
    public interface IOrdersRepository
    {
        public Task CreateOrderAsync(OrdersDTO model);

        public Task<List<OrdersDTO>> GetAllNewOrder();

        public  Task ConfirmOrder(Guid orderID);

        public Task CancelOrder(Guid orderID);


        public Task<List<OrdersDTO>> GetAllConfirmOrder();

        public  Task<List<OrdersDTO>> GetAllCancelOrder();


        public Task<OrdersDTO> Detail(Guid orderID);


        public Task<List<double>> GetTotalAmountByMonth();

        public Task<List<OrdersDTO>> GetOrdersByUserIdAndStatus(string status);

    }
    //

    public class OrdersRepository : IOrdersRepository
    {
        private readonly DbNet2Context _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClaimsPrincipal? _user;
        private readonly ICartRepository _cartRepository;

        public OrdersRepository(DbNet2Context context, IHttpContextAccessor httpContextAccessor, ICartRepository cartRepository)
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
                Status = "Chờ xác nhận",
                Address = model.Address,
                City = model.City,
                District = model.District,
                Ward = model.Ward,
                PhoneNumber = model.PhoneNumber,


            };

            var exist = await _cartRepository.GetOrCreateCartAsync(userID);

            if (exist == null)
            {
                throw new Exception("Cart is empty");
            }

            var cartItems = await _context.CartItem.Include(ci => ci.Products)
                .Where(ci => ci.CartID == exist.CartID).ToListAsync();


            var orderDetails = new List<OrderDetails>();
            foreach (var item in cartItems)
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductID == item.ProductID);

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



        public  async Task ConfirmOrder(Guid orderID)
        {
          
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderID == orderID);

            if(order == null)
            {
                throw new Exception("Order not found");
            }
            order.Status = "Đã xác nhận";

            _context.Orders.Update(order);


            
        }


        public async Task CancelOrder(Guid orderID)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderID == orderID);

            if (order == null)
            {
                throw new Exception("Order not found");
            }
            order.Status = "Đã hủy";

            _context.Orders.Update(order);

            _context.OrderDetails.Where(od => od.OrderID == orderID).ToList().ForEach(od =>
            {
                var colorSize = _context.ColorSizes.FirstOrDefault(cs => cs.ColorSizesID == od.ColorSizesID);
                colorSize.Quantity += od.Quantity;
                _context.ColorSizes.Update(colorSize);
            });
        }

        public async Task<List<OrdersDTO>> GetAllNewOrder()
        {
            var result = await _context.Orders.Where(c => c.Status == "Chờ xác nhận").Select(o => new OrdersDTO
            {
                OrderID = o.OrderID,
                OrderDate = o.OrderDate,
                fullName = o.FullName,
                Status = o.Status,
                Address = o.Address,
                City = o.City,
                District = o.District,
                Ward = o.Ward,
                PhoneNumber = o.PhoneNumber,
                TotalAmount = o.TotalAmount
            }).ToListAsync();


            return result;
        }

        public async Task<List<OrdersDTO>> GetAllConfirmOrder()
        {
            var result = await _context.Orders.Where(c => c.Status == "Đã xác nhận").Select(o => new OrdersDTO
            {
                OrderID = o.OrderID,
                OrderDate = o.OrderDate,
                fullName = o.FullName,
                Status = o.Status,
                Address = o.Address,
                City = o.City,
                District = o.District,
                Ward = o.Ward,
                PhoneNumber = o.PhoneNumber,
                TotalAmount = o.TotalAmount
            }).ToListAsync();


            return result;
        }

        public async Task<List<OrdersDTO>> GetAllCancelOrder()
        {
            var result = await _context.Orders.Where(c => c.Status == "Đã hủy").Select(o => new OrdersDTO
            {
                OrderID = o.OrderID,
                OrderDate = o.OrderDate,
                fullName = o.FullName,
                Status = o.Status,
                Address = o.Address,
                City = o.City,
                District = o.District,
                Ward = o.Ward,
                PhoneNumber = o.PhoneNumber,
                TotalAmount = o.TotalAmount
            }).ToListAsync();


            return result;
        }


        public async Task<OrdersDTO> Detail(Guid orderID)
        {
            var existOrder = await _context.Orders
                .Where(o => o.OrderID == orderID)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.ColorSizes)
                        .ThenInclude(cs => cs.Product)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.ColorSizes)
                        .ThenInclude(cs => cs.Sizes)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.ColorSizes)
                        .ThenInclude(cs => cs.Color)
                .FirstOrDefaultAsync();

            if (existOrder == null)
                return null;

            var order = new OrdersDTO
            {
                OrderID = existOrder.OrderID,
                OrderDate = existOrder.OrderDate,
                fullName = existOrder.FullName,
                Status = existOrder.Status,
                Address = existOrder.Address,
                City = existOrder.City,
                District = existOrder.District,
                Ward = existOrder.Ward,
                PhoneNumber = existOrder.PhoneNumber,
                TotalAmount = existOrder.TotalAmount,
                orderDetailDTO = existOrder.OrderDetails.Select(od => new OrderDetailDTO
                {
                    OrderID = od.OrderID,
                    ProductID = od.ProductID,
                    ColorSizeID = od.ColorSizesID,
                    ProductName = od.ColorSizes.Product.ProductName, // Lấy tên sản phẩm
                    Image = od.ColorSizes.Product.ImageProduct,      // Lấy ảnh sản phẩm
                    Price = od.Price,
                    SizeName = od.ColorSizes.Sizes.SizeName,
                    ColorName = od.ColorSizes.Color.ColorName,
                    Quantity = od.Quantity
                }).ToList()
            };

            return order;
        }
        public async Task<List<double>> GetTotalAmountByMonth()
        {
            var result = await _context.Orders
                .Where(o => o.Status == "Đã xác nhận")
                .GroupBy(o => o.OrderDate.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    TotalAmount = g.Sum(o => o.TotalAmount)
                })
                .OrderBy(g => g.Month)
                .ToListAsync();

            var chartData = new List<double>();
            double runningTotal = 0;

            foreach (var item in result)
            {
                runningTotal += item.TotalAmount;
                chartData.Add(runningTotal);
            }

            return chartData;
        }

        // Lấy danh sách đơn hàng theo UserID và Status
        public async Task<List<OrdersDTO>> GetOrdersByUserIdAndStatus(string status)
        {
            var userId = _httpContextAccessor.HttpContext?.Session.GetString("UserID");
            var result = await _context.Orders
                .Where(o => o.UserID == Guid.Parse(userId) && o.Status == status)
                .Select(o => new OrdersDTO
                {
                    OrderID = o.OrderID,
                    OrderDate = o.OrderDate,
                    fullName = o.FullName,
                    Status = o.Status,
                    Address = o.Address,
                    City = o.City,
                    District = o.District,
                    Ward = o.Ward,
                    PhoneNumber = o.PhoneNumber,
                    TotalAmount = o.TotalAmount
                })
                .ToListAsync();

            return result;
        }
       
        
        


    }
}
