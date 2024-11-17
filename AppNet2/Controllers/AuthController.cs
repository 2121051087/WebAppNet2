using Microsoft.AspNetCore.Mvc;
using WebAppNet2.Models.Auth;
using WebAppNet2.Repositories;

namespace WebAppNet2.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthRepository _authRepository;

        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LogIn(LogInVM model)
        {
            if (ModelState.IsValid)
            {
                var result = await _authRepository.LogInAsync(model, HttpContext);
                
             

                if (result != null)
                {
                    dynamic userResult = result;
                    var roles = userResult.roles as IList<string>;
                    if (roles.Contains("Administrator"))
                    {
                        return RedirectToAction("Index", "DashBoard", new { area = "Admin" });
                    }
                    return RedirectToAction("Index", "Home" ,new {area = "Customer"});
                }

                ModelState.AddModelError("", "Invalid credentials");
            }
            return View(model);
        }
        public  IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                var result = await _authRepository.RegisterAsync(model);
                if (result != null)
                {
                    return RedirectToAction("LogIn", "Auth");
                }
                ModelState.AddModelError("", "Register error");
            }
            return View(model);
        }
      
    }
}
