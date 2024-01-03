using AppData.IServices;
using AppData.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PRO219_WebsiteBanDienThoai_FPhone.ViewModel;

namespace PRO219_WebsiteBanDienThoai_FPhone.Controllers
{
    public class BlogsController : Controller
    {
        private readonly HttpClient _httpClient;
        private IBlogService _service;

        public BlogsController(HttpClient httpClient, IBlogService service)
        {
            _httpClient = httpClient;
            _service = service;
        }
        public IActionResult Index()
        {
            AdBlogViewModel model = new AdBlogViewModel();
            model.Records = _service.GetAll(model.SearchData, model.ListOptions);
            return View(model);
        }
        [HttpGet]

        public async Task<IActionResult> BlogDetail(Guid Id)
        {
            AdBlogViewModel model = new AdBlogViewModel();
            model.DetailModel = _service.Details(Id);
            model.Records = _service.GetAll(model.SearchData, model.ListOptions).Where(c =>c.Id!=Id).OrderByDescending(c =>c.CreatedDate).Take(3).ToList();
            return View(model);
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
