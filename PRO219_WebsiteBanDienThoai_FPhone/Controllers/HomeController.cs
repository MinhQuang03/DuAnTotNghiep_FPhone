using AppData.FPhoneDbContexts;
using AppData.IServices;
using AppData.Models;
using AppData.ViewModels.Phones;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PRO219_WebsiteBanDienThoai_FPhone.ViewModel;

namespace PRO219_WebsiteBanDienThoai_FPhone.Controllers;

public class HomeController : Controller
{
    private readonly HttpClient _client;
    private IVwPhoneService _phoneService;
    private IVwTop5PhoneServices _top5PhoneService;
    private readonly ILogger<HomeController> _logger;
    private FPhoneDbContext _context;
    private IHttpContextAccessor _accessor;
    public HomeController(ILogger<HomeController> logger, HttpClient client, IVwPhoneService phoneService, IVwTop5PhoneServices top5PhoneService, IHttpContextAccessor accessor)
    {
        _context = new FPhoneDbContext();
        _logger = logger;
        _client = client;
        _phoneService = phoneService;
        _accessor = accessor;
        _top5PhoneService = top5PhoneService;
    }


    public async Task<IActionResult> Index()
    {
        HomeGroupViewModel model = new HomeGroupViewModel();
        model.vPhoneGroup = _phoneService.listVwPhoneGroup(model._VW_Phone_Group).Take(20).ToList();
        model.vTop5 = _top5PhoneService.listVwTop5PhoneGroup();
        return View(model);
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
}