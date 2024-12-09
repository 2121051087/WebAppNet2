using AppNet2.Models;
using Microsoft.AspNetCore.Mvc;
using WebAppNet2.Infrastructures;
using WebAppNet2.Infrastructures.UnitOfWork;
using WebAppNet2.Models.DTO;

namespace WebAppNet2.Controllers
{

    [Route("Customer/[Controller]/[Action]")]
    [RoleAuthorize(AppRole.Customer)]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }
        // Checkout 

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var listCartItem = await _unitOfWork.CartRepository.ListItemCartAsync();

            var orders = new OrdersDTO
            {
                cartItemDTOs = listCartItem,
                TotalAmount = listCartItem.Sum(item => item.Price * item.Quantity)
            };
            return View(orders);
        }
       
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrdersDTO model)
        {
           

            if (model != null)
            {
                await _unitOfWork.OrdersRepository.CreateOrderAsync(model);

                await _unitOfWork.CartRepository.ClearCartAsync();

                await _unitOfWork.SaveChangeAsync();

                // Trả về phản hồi JSON với thông tin thành công
                return Ok(new { success = true, message = "Order created successfully." });
            }

            // Trả về NotFound nếu model không hợp lệ
            return NotFound(new { success = false, message = "Order not found." });
        }
    }
}
