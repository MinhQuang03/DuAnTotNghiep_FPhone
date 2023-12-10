using AppData.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PRO219_WebsiteBanDienThoai_FPhone.ViewModel;

namespace PRO219_WebsiteBanDienThoai_FPhone.Controllers
{
    public class BlogsController : Controller
    {
        private readonly HttpClient _httpClient;
        public BlogsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]

        public async Task<IActionResult> BlogDetail(Guid Id)
        {
            var datajson = await _httpClient.GetStringAsync($"api/Blog/get");
            var ctblog = JsonConvert.DeserializeObject<List<Blog>>(datajson);
            var lst = ctblog.Where(c => c.Id == Id).ToList();
            return View(lst);
        }
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
