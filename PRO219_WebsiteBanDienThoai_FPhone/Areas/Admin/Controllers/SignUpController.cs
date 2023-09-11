using Microsoft.AspNetCore.Mvc;

namespace PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SignUpController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
