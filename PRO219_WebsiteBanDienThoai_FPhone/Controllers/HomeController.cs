using AppData.FPhoneDbContexts;
using AppData.IServices;
using AppData.Models;
using AppData.ViewModels.Phones;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace PRO219_WebsiteBanDienThoai_FPhone.Controllers;

public class HomeController : Controller
{
    private readonly HttpClient _client;
    private IVwPhoneService _phoneService;
    private readonly ILogger<HomeController> _logger;
    private FPhoneDbContext _context;
    public HomeController(ILogger<HomeController> logger, HttpClient client, IVwPhoneService phoneService)
    {
        _context = new FPhoneDbContext();
        _logger = logger;
        _client = client;
        _phoneService = phoneService;
    }


    public async Task<IActionResult> Index()
    {
        VW_Phone_Group model = new VW_Phone_Group();
	    var data= _phoneService.listVwPhoneGroup(model);
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

    //public async Task<IActionResult> BlogDetail(Guid id)
    //{
    //    var datajson = await _client.GetStringAsync("api/Blog/getById/{id}");
    //    var detail = JsonConvert.DeserializeObject<List<Blog>>(datajson);
    //    return View(detail);
    //}

    //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    //public IActionResult Error()
    //{
    //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    //}
}