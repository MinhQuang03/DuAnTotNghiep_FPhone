using Microsoft.AspNetCore.Mvc;

namespace PRO219_WebsiteBanDienThoai_FPhone.Controllers
{
    public class BlogController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7129/api");
        private readonly HttpClient _httpClient;
        public BlogController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult BlogManagement()
        {
            return View();
        }
        public IActionResult CreateBlog()
        {
            return View();
        }
        public IActionResult BlogDetail()
        {
            return View();
        }
        public IActionResult EditBlog()
        {
            return View();
        }
        public IActionResult DeleteBlog() 
        { 
            return View(); 
        }

    }
}
