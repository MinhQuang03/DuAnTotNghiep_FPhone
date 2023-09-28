using AppData.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace PRO219_WebsiteBanDienThoai_FPhone.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
 
        private HttpClient _client;
       

        public HomeController(ILogger<HomeController> logger,HttpClient client)
        {
            _logger = logger;
            _client = client;
        }

        public async Task<IActionResult> Index()
        {
            string datajson = await _client.GetStringAsync("api/PhoneDetaild/get");
            List<PhoneDetaild> ctsp = JsonConvert.DeserializeObject<List<PhoneDetaild>>(datajson);
            return View(ctsp);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> ShowPhone()
        {
            string datajson = await _client.GetStringAsync("api/PhoneDetaild/get");
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