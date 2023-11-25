using AppData.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Filters;

namespace PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Controllers;

[Area("Admin")]
[AuthenFilter]
public class AccountsController : Controller
{
    private readonly HttpClient _client;

    public AccountsController(HttpClient client)
    {
        _client = client;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Account()
    {
        var response = await _client.GetAsync("/api/Accounts/get-all-staff");
        if (response.IsSuccessStatusCode)
            if (UserClaim.HasRole(User, "Admin"))
            {
                var content = await response.Content.ReadAsStringAsync();
                var accounts = JsonConvert.DeserializeObject<List<ApplicationUser>>(content);
                return View(accounts);
            }

        return BadRequest();
    }

    public async Task<RedirectResult> LogOut()
    {
        
        var authenticationProperties = new AuthenticationProperties
        {
            ExpiresUtc = DateTimeOffset.UtcNow.AddSeconds(20) // Thiết lập thời gian hết hạn sau khi đăng xuất
        };

        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme, authenticationProperties);
        Response.Cookies.Delete(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectPermanent("/home");
    }

    public IActionResult AddSildeShow()
    {
        return View();
    }
}