using Microsoft.AspNetCore.Mvc;

namespace PRO219_WebsiteBanDienThoai_FPhone.Controllers
{
    public class BlogController : Controller
    {
        private readonly HttpClient _httpClient;
        public BlogController(HttpClient httpClient)
        {
            _httpClient = httpClient;
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
