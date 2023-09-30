using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using AppData.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Utilities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SignUpController : Controller
    {
        private readonly HttpClient _client;
        private IHttpContextAccessor _contextAccessor;
        public SignUpController(HttpClient client, IHttpContextAccessor contextAccessor)
        {
            _client = client;
            _contextAccessor = contextAccessor; 
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
      
           
           
            if (UserClaim.HasRole(User, "Admin"))
            {
                var token = _contextAccessor.HttpContext.Request.Cookies["token"];
                var content = await _client.GetStringAsync("/api/AccountStaff/get-all-staff");
                var account = JsonConvert.DeserializeObject<List<ApplicationUser>>(content).Count;
                model.Status = 1;
                model.ImageUrl = "";
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token);
                var result = await _client.PostAsJsonAsync("/api/AccountStaff/SignUp", model);
                if (result.IsSuccessStatusCode)
                {
                    if (account == 0) // khi chưa có tài khoản nào ( signup với role admin )
                    {
                        return RedirectToAction("Login", "LogIn");
                    }

                    return RedirectToAction("Accounts", "Home"); // signup vs role staff
                }
            }

            return RedirectToAction("Index");
        }
    
    }
}
