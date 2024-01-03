using AppData.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Filters;
using System.Text;
using AppData.IServices;
using PRO219_WebsiteBanDienThoai_FPhone.ViewModel;

namespace PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[AuthenFilter]
    public class BlogController : Controller
    {
        public readonly HttpClient _httpClient;
        private IBlogService _service;

        public BlogController(HttpClient httpClient, IBlogService service)
        {
            _httpClient = httpClient;
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            AdBlogViewModel model = new AdBlogViewModel();
            model.Records = _service.GetAll(model.SearchData, model.ListOptions);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(AdBlogViewModel model)
        {
            model.Records = _service.GetAll(model.SearchData, model.ListOptions);
            return View(model);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Blog obj, IFormFile file)
        {
            if (file != null && file.Length > 0) // khong null va khong trong 
            {
                var fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                obj.Images = "/img/" + fileName;
            }
            obj.CreatedDate = DateTime.Now;
            var data = _service.Add(obj);
            if (data != null)
            {
                return RedirectToAction("Index");
            }

            return View(obj);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            Blog model = new Blog();
            model = _service.Details(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Blog obj, IFormFile file)
        {
            if (file != null && file.Length > 0) // khong null va khong trong 
            {
                var fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                obj.Images = "/img/" + fileName;
            }

            obj.CreatedDate = DateTime.Now;
            var data = _service.Update(obj);
            if (data != null)
            {
                return RedirectToAction("Index");
            }

            return View(obj);
        }


        public async Task<IActionResult> Delete(Guid id)
        {

            var response = await _httpClient.DeleteAsync($"api/Blog/delete/{id}");
            return RedirectToAction("Index");
        }
        
    }
}
