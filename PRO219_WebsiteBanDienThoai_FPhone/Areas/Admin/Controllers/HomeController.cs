using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AppData.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private HttpClient _client;
        private IHttpContextAccessor _contextAccessor;

        public HomeController(HttpClient client, IHttpContextAccessor contextAccessor)
        {
            _client = client;
            _contextAccessor = contextAccessor;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Accounts()
        {
            var response = await _client.GetAsync("/api/AccountStaff/get-all-staff");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var accounts = JsonConvert.DeserializeObject<List<ApplicationUser>>(content);
                return View(accounts);
            }
            return View(null);
        }
        public List<Claim> GetClaimsFromTokenInCookie()
        {
            var token = _contextAccessor.HttpContext.Request.Cookies["token"];
            if (string.IsNullOrEmpty(token))
            {
                // Handle the case where token is not available in the cookie.
                return null;
            }
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            return jwtToken.Claims.ToList();
        }

    }
}
