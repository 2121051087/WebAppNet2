﻿using AppNet2.Models;
using Microsoft.AspNetCore.Mvc;
using WebAppNet2.Infrastructures;
using WebAppNet2.Infrastructures.UnitOfWork;
using WebAppNet2.Models.DTO;

namespace WebAppNet2.Areas.Customer.Controllers
{
    [Area("Customer")]
    [RoleAuthorize(AppRole.Customer)]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var model = new HomeViewModel
            {
                ListProductsByCatgory = _unitOfWork.CategoriesRepository.GetProductsByCategory().Result,
                CategoriesVM = _unitOfWork.CategoriesRepository.GetAllCategories().Result,

            };



            return View(model);
        }

        public IActionResult ProductDetails(Guid id)
        {
            var product = _unitOfWork.ProductRepository.GetProductById(id).Result;

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult SizeGuideTable()
        {
            return View();
        }


        public IActionResult StoreSystem()
        {
            return View();
        }

        [HttpGet]

        public async Task<IActionResult> OrderTracking(string status)
        {
            if (status == null)
            {
                status = "Chờ xác nhận";
            }

            var data = await _unitOfWork.OrdersRepository.GetOrdersByUserIdAndStatus(status);

            return View(data);
        }
        [HttpGet("{orderID}")]
        public async Task<IActionResult> ConfirmOrder(Guid orderID)
        {
            await _unitOfWork.OrdersRepository.ConfirmOrder(orderID);

            if (orderID == Guid.Empty)
            {
                return BadRequest();
            }
            await _unitOfWork.OrdersRepository.ConfirmOrder(orderID);

            await _unitOfWork.SaveChangeAsync();

            return RedirectToAction("OrderTracking");
        }
    }
}

