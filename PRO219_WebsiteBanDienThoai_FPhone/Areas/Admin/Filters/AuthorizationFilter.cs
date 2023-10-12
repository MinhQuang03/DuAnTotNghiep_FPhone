using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Filters;

public class AuthorizationFilter : Attribute, IActionFilter
{
    private readonly string _role;

    public AuthorizationFilter(string role)
    {
        _role = role;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.HttpContext.User.Identity.IsAuthenticated == false) context.Result = new UnauthorizedResult();

        if (!context.HttpContext.User.HasClaim("role", _role)) context.Result = new UnauthorizedResult();
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}