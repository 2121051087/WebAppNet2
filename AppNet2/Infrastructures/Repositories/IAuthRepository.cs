using Microsoft.AspNetCore.Identity;
using WebAppNet2.Models.Auth;

namespace WebAppNet2.Infrastructures.Repositories
{
    public interface IAuthRepository
    {
        public Task<IdentityResult> RegisterAsync(RegisterVM model);

        public Task<object> LogInAsync(LogInVM model, HttpContext httpContext);

        public Task LogOutAsync();
    }
}
