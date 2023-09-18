using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using AppData.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Utilities;

namespace PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private HttpClient _client;
        private IHttpContextAccessor _contextAccessor;
        private Utility _utility;
    

        public HomeController(HttpClient client, IHttpContextAccessor contextAccessor, Utility utility)
        {
            _client = client;
            _contextAccessor = contextAccessor;
            _utility = utility;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Accounts()
        {
            var token = _contextAccessor.HttpContext.Request.Cookies["token"];
            if (string.IsNullOrEmpty(token))
            {
                // Handle the case where token is not available in the cookie.
                return View(null);
            }
            var response = await _client.GetAsync("/api/AccountStaff/get-all-staff");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var accounts = JsonConvert.DeserializeObject<List<ApplicationUser>>(content);
                return View(accounts);
            }
            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> SignOut()
        {
            await _contextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToPage("/Admin");
        }
    }
}