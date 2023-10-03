using AppData.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PRO219_WebsiteBanDienThoai_FPhone.Models;

namespace PRO219_WebsiteBanDienThoai_FPhone.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly HttpClient _client;


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