using System.Data;
using AppData.Models;
using Microsoft.AspNetCore.Mvc;

namespace PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private HttpClient _client;

        
        public HomeController(HttpClient client)
        {
            _client = client;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Accounts()
        {
            return View();
        }
       

    }
}
