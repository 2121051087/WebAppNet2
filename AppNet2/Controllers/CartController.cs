using AppNet2.Models;
using Microsoft.AspNetCore.Mvc;
using WebAppNet2.Infrastructures;
using WebAppNet2.Infrastructures.UnitOfWork;
using WebAppNet2.Models.DTO;
using WebAppNet2.Models.Entities.Catalog;

namespace WebAppNet2.Controllers
{

    [Route("Customer/[Controller]/[Action]")]
    [RoleAuthorize(AppRole.Customer)]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddItemToCart([FromBody] CartItemDTO model)
        {
            // Kiểm tra xác thực người dùng từ Session
            var userID = HttpContext.Session.GetString("UserID");

            if (string.IsNullOrEmpty(userID))
            {
                // Nếu không có UserID trong session, trả về Unauthorized (401)
                return Unauthorized(new { message = "User is not authenticated." });
            }

         
            if(model.ColorSizeID.ToString() == "00000000-0000-0000-0000-000000000000")
            {
                model.ColorSizeID = await _unitOfWork.ProductRepository.GetColorSizeID(model);
            }

            if (model != null)
            {
                await _unitOfWork.CartRepository.AddItemToCartAsync(model);

                await _unitOfWork.SaveChangeAsync();

                // Trả về phản hồi JSON với thông tin thành công
                return Ok(new { success = true, message = "Item added to cart successfully." });
            }

            // Trả về NotFound nếu model không hợp lệ
            return NotFound(new { success = false, message = "Item not found." });
        }

    }
}
