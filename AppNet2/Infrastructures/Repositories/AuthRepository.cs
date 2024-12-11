using AppNet2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAppNet2.Models.Auth;



namespace WebAppNet2.Infrastructures.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ICartRepository _cartRepository;

        public AuthRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, RoleManager<IdentityRole> roleManager ,ICartRepository cartRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager;
            _cartRepository = cartRepository;
        }


        public async Task<object> LogInAsync(LogInVM model, HttpContext httpContext)
        {
            // Tìm người dùng qua email
            var user = await _userManager.FindByEmailAsync(model.Email);

            // Kiểm tra thông tin đăng nhập
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return null;
            }
            var cart = await _cartRepository.GetOrCreateCartAsync(user.Id);

            // Đăng nhập thành công: lưu thông tin vào Session
            httpContext.Session.SetString("UserID", user.Id);
            httpContext.Session.SetString("UserEmail", user.Email);

            // Lấy danh sách vai trò và lưu vào Session
            var userRoles = await _userManager.GetRolesAsync(user);
            httpContext.Session.SetString("UserRoles", string.Join(",", userRoles));

            return new
            {
                userId = user.Id,
                email = user.Email,
                roles = userRoles,

            };

        }

        public async Task<IdentityResult> RegisterAsync(RegisterVM model)
        {
            var user = new ApplicationUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync(AppRole.Customer))
                {
                    await _roleManager.CreateAsync(new IdentityRole(AppRole.Customer));
                }

                await _userManager.AddToRoleAsync(user, AppRole.Customer);
                //if (!await _roleManager.RoleExistsAsync(AppRole.Admin))
                //{
                //    await _roleManager.CreateAsync(new IdentityRole(AppRole.Admin));
                //}

                //await _userManager.AddToRoleAsync(user, AppRole.Admin);
            }
            return result;
        }

        public async Task LogOutAsync()
        {
            await _signInManager.SignOutAsync();

        }
        


        public async Task<int> CountCustomerAccountsAsync()
        {
            var customers = await _userManager.GetUsersInRoleAsync(AppRole.Customer);
            return customers.Count;
        }

    }
}
