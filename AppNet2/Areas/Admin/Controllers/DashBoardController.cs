using AppNet2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppNet2.Infrastructures;
using WebAppNet2.Infrastructures.UnitOfWork;

namespace WebAppNet2.Areas.Admin.Controllers
{
    [Area("Admin")]
    [RoleAuthorize(AppRole.Admin)]
    public class DashBoardController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashBoardController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            

            var chartData = await _unitOfWork.OrdersRepository.GetTotalAmountByMonth();

            return View(chartData);
        }
    }
}
