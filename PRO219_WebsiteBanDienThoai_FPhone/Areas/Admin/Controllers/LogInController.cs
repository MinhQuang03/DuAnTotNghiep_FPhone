using AppData.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Utilities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Principal;

namespace PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LogInController : Controller
    {
        private HttpClient _client;
        private IHttpContextAccessor _contextAccessor;
        private Utility _utility;


        public LogInController(HttpClient client, IHttpContextAccessor contextAccessor,Utility utility)
        {
            _client = client;
            _contextAccessor = contextAccessor;
            _utility = utility;
        }
        public IActionResult Index()
        { 
            //_contextAccessor.HttpContext?.Response.Cookies.Delete("token");
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
                var claims = _utility.GetClaimsFromTokenInCookie("token");
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await _contextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index");
        }
    }
}