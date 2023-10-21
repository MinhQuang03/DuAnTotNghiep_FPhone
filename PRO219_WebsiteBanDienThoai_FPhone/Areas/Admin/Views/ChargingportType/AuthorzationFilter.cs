using Microsoft.AspNetCore.Mvc.Filters;

namespace PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Filters
{
    public class AuthorzationFilter :Attribute,IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            throw new NotImplementedException();
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            throw new NotImplementedException();
        }
    }
}
