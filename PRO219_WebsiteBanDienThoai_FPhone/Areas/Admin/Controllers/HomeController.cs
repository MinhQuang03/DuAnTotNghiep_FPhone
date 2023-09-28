using AppData.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Utilities;

//using static PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Utilities.Utility;


namespace PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Controllers;

[Area("Admin")]
public class HomeController : Controller
{
    private readonly HttpClient _client;
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
            if (UserClaim.HasRole(User, "Admin"))
            {
                var content = await response.Content.ReadAsStringAsync();
                var accounts = JsonConvert.DeserializeObject<List<ApplicationUser>>(content);
                return View(accounts);
            }

        return BadRequest();
    }

    public async Task<IActionResult> LogOut()
    {
        var authenticationProperties = new AuthenticationProperties
        {
            ExpiresUtc = DateTimeOffset.UtcNow.AddSeconds(20) // Thiết lập thời gian hết hạn sau khi đăng xuất
        };
        await HttpContext.SignOutAsync("token", authenticationProperties);
        return RedirectToAction("Login", "LogIn");
    }
}