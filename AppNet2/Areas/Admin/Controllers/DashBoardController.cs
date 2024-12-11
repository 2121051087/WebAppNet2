using AppNet2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppNet2.Infrastructures;
using WebAppNet2.Infrastructures.Repositories;
using WebAppNet2.Infrastructures.UnitOfWork;
using WebAppNet2.Models.DTO;

namespace WebAppNet2.Areas.Admin.Controllers
{
    [Area("Admin")]

    [RoleAuthorize(AppRole.Admin)]
    [Route("Admin/[Controller]/[Action]")]
    public class DashBoardController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthRepository _repo;

        public DashBoardController(IUnitOfWork unitOfWork, IAuthRepository repo)
        {
            _unitOfWork = unitOfWork;
            _repo = repo;
        }

        public async Task<IActionResult> Index()
        {

            var model = new DashBoardModelView
            {
                chartDataetTotalAmountByMonth = await _unitOfWork.OrdersRepository.GetTotalAmountByMonth(),

                chartDataGetTotalAmountByWeek = await _unitOfWork.OrdersRepository.GetTotalAmountByWeek(),

                CountConfirmedOrders = await _unitOfWork.OrdersRepository.CountConfirmedOrders(),

                CountSoldProducts = await _unitOfWork.OrdersRepository.CountSoldProducts(),

                CountCategories = await _unitOfWork.CategoriesRepository.CountCategories(),

                CountProduct = await _unitOfWork.ProductRepository.CountProduct(),

                CountUsers = await _repo.CountCustomerAccountsAsync(),
            };


            return View(model);
        }
    }
}
