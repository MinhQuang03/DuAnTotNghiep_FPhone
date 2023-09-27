using AppData.FPhoneDbContexts;
using AppData.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PRO219_WebsiteBanDienThoai_FPhone.Models;
using System.Diagnostics;

namespace PRO219_WebsiteBanDienThoai_FPhone.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        string domain = "https://localhost:7129/";
        HttpClient client = new HttpClient();
        FPhoneDbContext _context;
        FPhoneDbContext ShoppingDbContext;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _context = new FPhoneDbContext();
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Domain = domain;
            client.BaseAddress = new Uri(domain);
            string datajson = await client.GetStringAsync("api/PhoneDetaild/get");
            List<PhoneDetaild> ctsp = JsonConvert.DeserializeObject<List<PhoneDetaild>>(datajson);
            return View(ctsp);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> ShowPhone()
        {
            ViewBag.Domain = domain;
            client.BaseAddress = new Uri(domain);
            string datajson = await client.GetStringAsync("api/PhoneDetaild/get");
            List<PhoneDetaild> ctsp = JsonConvert.DeserializeObject<List<PhoneDetaild>>(datajson);
            return View(ctsp);
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}