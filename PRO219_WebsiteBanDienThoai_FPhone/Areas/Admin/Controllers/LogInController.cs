using AppData.Models;
using Microsoft.AspNetCore.Mvc;

namespace PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LogInController : Controller
    {
        private HttpClient _client;

        public LogInController(HttpClient client)
        {
            _client = client;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(SignInModel model)
        {
            var result = await _client.PostAsJsonAsync("/api/AccountStaff/SignIn", model);
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Index","Home");
            }

            return RedirectToAction("Login");
        }

    }
}
