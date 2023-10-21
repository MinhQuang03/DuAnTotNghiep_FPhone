using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Filters;

public class AuthenFilter : Attribute, IActionFilter
{

    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.HttpContext.User.Identity.IsAuthenticated == false || context.HttpContext.User.HasClaim("role","User")) context.Result = new RedirectToRouteResult(new RouteValueDictionary(new {action="Index",controller="Home",area=""}));
        //if (!context.HttpContext.User.HasClaim("role", _role)) context.Result = new UnauthorizedResult();
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}