using AppData.IRepositories;
using AppData.IServices;
using AppData.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PRO219_WebsiteBanDienThoai_FPhone.ViewModel;

namespace PRO219_WebsiteBanDienThoai_FPhone.Controllers;

public class HomeController : Controller
{
    private readonly HttpClient _client;
    private readonly ILogger<HomeController> _logger;
    private IVwPhoneService _VwPhoneService;

    public HomeController(ILogger<HomeController> logger, HttpClient client,IVwPhoneService VwPhoneService)
    {
        _logger = logger;
        _client = client;
        _VwPhoneService = VwPhoneService;
    }


    public async Task<IActionResult> Index()
    {
    
        var data = _VwPhoneService.listVwPhoneGroup();
        return View(data);
    }

   

    public async Task<IActionResult> LogOut()
    {
        var authenticationProperties = new AuthenticationProperties
        {
            ExpiresUtc = DateTimeOffset.UtcNow.AddSeconds(20) // Thiết lập thời gian hết hạn sau khi đăng xuất
        };
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme, authenticationProperties);
        return RedirectToAction("Index");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public async Task<IActionResult> ShowPhone()
    {
        var datajson = await _client.GetStringAsync("api/PhoneDetaild/get");
        var ctsp = JsonConvert.DeserializeObject<List<PhoneDetaild>>(datajson);
        return View(ctsp);
    }

   

    //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    //public IActionResult Error()
    //{
    //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    //}
}