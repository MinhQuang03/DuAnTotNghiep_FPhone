using AppData.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Utilities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Principal;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Newtonsoft.Json;

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
        public async Task<IActionResult> Index()
        {
            var result = await _client.GetStringAsync("/api/AccountStaff/get-all-staff");
            var accounts = JsonConvert.DeserializeObject<List<ApplicationUser>>(result).Count;
            if (accounts == 0)
            {
                return RedirectToAction("SignUpAdmin","SignUp");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(SignInModel model)
        {
            var handler = new JwtSecurityTokenHandler();
            var result = await _client.PostAsJsonAsync("/api/AccountStaff/SignIn", model);
            var options = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7) // Thời gian hết hạn của cookie
            };

            if (result.IsSuccessStatusCode)
            {
                var token = await result.Content.ReadAsStringAsync();

                var claimsPrincipal = handler.ReadJwtToken(token);

                //List<Claim> claims = _utility.GetClaimsFromTokenInCookie("token");
                var claims = claimsPrincipal.Claims;
                var identity = new ClaimsIdentity(claims, "token");
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync("token", principal);
                HttpContext.Response.Cookies.Append("token", token, options);
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index");
        }
    }
}