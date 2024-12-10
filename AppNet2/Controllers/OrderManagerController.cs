using AppNet2.Models;
using Microsoft.AspNetCore.Mvc;
using WebAppNet2.Infrastructures;
using WebAppNet2.Infrastructures.UnitOfWork;

namespace WebAppNet2.Controllers
{
    [Route("Admin/[Controller]/[Action]")]
    [RoleAuthorize(AppRole.Admin)]
    public class OrderManagerController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderManagerController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _unitOfWork.OrdersRepository.GetAllNewOrder();
            return View(result);
        }



        [HttpGet]
        public async Task<IActionResult> ListConfirmedOrder()
        {
            var result = await _unitOfWork.OrdersRepository.GetAllConfirmOrder();
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> ListCancelOrder()
        {
            var result = await _unitOfWork.OrdersRepository.GetAllCancelOrder();
            return View(result);
        }

        [HttpGet("{orderID}")]
        public async Task<IActionResult> Confirm(Guid orderID)
        {
            if(orderID == Guid.Empty)
            {
                return BadRequest();
            }
            await _unitOfWork.OrdersRepository.ConfirmOrder(orderID);

            await _unitOfWork.SaveChangeAsync();

            return RedirectToAction("Index");

        }

        [HttpGet("{orderID}")]
        public async Task<IActionResult> Cancel(Guid orderID)
        {
            if(orderID == Guid.Empty)
            {
                return BadRequest();
            }
           await  _unitOfWork.OrdersRepository.CancelOrder(orderID);

           await _unitOfWork.SaveChangeAsync();

           return RedirectToAction("Index");
        }


        [HttpGet("{orderID}")]
        public async Task<IActionResult> Detail(Guid orderID)
        {
            var result = await _unitOfWork.OrdersRepository.Detail(orderID);

            return View(result);
        }
    }
}
