using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAppNet2.Infrastructures
{
    public class RoleAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string _role;

        public RoleAuthorizeAttribute(string role)
        {
            _role = role;

        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var roles = context.HttpContext.Session.GetString("UserRoles");

            if (string.IsNullOrEmpty(roles) || !roles.Split(',').Contains(_role))
            {
                // Nếu không có vai trò yêu cầu, trả về lỗi 403 Forbidden.
                context.Result = new ForbidResult();
            }
        }
    }
}
