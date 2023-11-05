using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AppData.Models;
using AppData.ViewModels.Accounts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PRO219_WebsiteBanDienThoai_FPhone.Models;

namespace PRO219_WebsiteBanDienThoai_FPhone.Controllers;

public class HomeController : Controller
{
    private readonly HttpClient _client;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, HttpClient client)
    {
        _logger = logger;
        _client = client;
    }


    public async Task<IActionResult> Index()
    {
        var datajson = await _client.GetStringAsync("api/PhoneDetaild/get");
        var ctsp = JsonConvert.DeserializeObject<List<PhoneDetaild>>(datajson);

        var lstspView = from a in ctsp
            group a by new
            {
                a.Phones.PhoneName,
                a.Phones.Id,
                a.Phones.Image
            }
            into b
            select new ProductView
            {
                IdProduct = b.Key.Id,
                ProductName = b.Key.PhoneName,
                Price = b.Select(c => c.Price).Min().ToString("C0") + " - " +
                        b.Select(c => c.Price).Max().ToString("C0"),
                Image = b.Key.Image
            };
        
        return View(lstspView);
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

   public async Task<IActionResult> BlogList()
    {
        var datajson = await _client.GetStringAsync("api/Blog/get");
        var blog = JsonConvert.DeserializeObject<List<Blog>>(datajson);

        var lstblogView = from a in blog
                          group a by new
                        {
                            a.Id,
                            a.Title,
                            a.CreatedDate,
                            a.Content
                        }
            into b
                        select new BlogView
                        {
                            Id = b.Key.Id,
                            Title = b.Key.Title,
                            CreatedDate = b.Key.CreatedDate,
                            Content = b.Key.Content
                        };

        return View(lstblogView);

    }
    public async Task<IActionResult> BlogDetail(Guid id)
    {
        var datajson = await _client.GetStringAsync("api/Blog/getById/{id}");
        var detail = JsonConvert.DeserializeObject<List<Blog>>(datajson);
        return View(detail);
    }

    //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    //public IActionResult Error()
    //{
    //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    //}
}