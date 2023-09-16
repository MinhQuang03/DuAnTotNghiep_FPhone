using AppData.Models;
using Microsoft.AspNetCore.Mvc;

namespace PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LogInController : Controller
    {
        private HttpClient _client;
        private IHttpContextAccessor _contextAccessor;


        public LogInController(HttpClient client, IHttpContextAccessor contextAccessor)
        {
            _client = client;
            _contextAccessor = contextAccessor;

        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(SignInModel model)
        {
            var result = await _client.PostAsJsonAsync("/api/AccountStaff/SignIn", model);
            var options = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7) // Thời gian hết hạn của cookie
            };
            var token = await result.Content.ReadAsStringAsync();
            _contextAccessor.HttpContext.Response.Cookies.Append("token", token, options);
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Index","Home");
            }
            return RedirectToAction("Login");
        }
    }
}
