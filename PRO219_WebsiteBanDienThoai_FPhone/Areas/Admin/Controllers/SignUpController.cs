using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using AppData.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SignUpController : Controller
    {
        private readonly HttpClient _client;

        public SignUpController(HttpClient client)
        {
            _client = client;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SignUpAdmin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpModel model)  
        {
            var content = await _client.GetStringAsync("/api/AccountStaff/get-all-staff");
            var account = JsonConvert.DeserializeObject<List<ApplicationUser>>(content).Count;
            model.Status = 1;
            model.ImageUrl = "";
            var result = await  _client.PostAsJsonAsync("/api/AccountStaff/SignUp", model);
            if (result.IsSuccessStatusCode)
            {
                if (account == 0) // khi chưa có tài khoản nào ( signup với role admin )
                {
                    return RedirectToAction("Index","LogIn");
                }
                return RedirectToAction("Accounts","Home"); // signup vs role staff
            }

            return RedirectToAction("Index");
        }
    
    }
}
