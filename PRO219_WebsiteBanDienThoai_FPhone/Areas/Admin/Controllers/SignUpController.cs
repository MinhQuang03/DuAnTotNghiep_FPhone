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
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpModel model)
        {
            model.Status = 1;
            model.ImageUrl = "";
            var result = await  _client.PostAsJsonAsync("/api/AccountStaff/SignUp", model);
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Accounts","Home");
            }
            
            return RedirectToAction("Index");
        }
    
    }
}
