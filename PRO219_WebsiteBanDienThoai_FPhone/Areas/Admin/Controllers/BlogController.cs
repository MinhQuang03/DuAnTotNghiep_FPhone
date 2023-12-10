using AppData.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Filters;
using System.Text;

namespace PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[AuthenFilter]
    public class BlogController : Controller
    {
        public readonly HttpClient _httpClient;
        public BlogController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            var datajson = await _httpClient.GetStringAsync("api/Blog/get");
            var obj = JsonConvert.DeserializeObject<List<Blog>>(datajson);
            return View(obj);
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
            var jsonData = JsonConvert.SerializeObject(obj);
            HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/Blog/add", content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View(jsonData);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var datajson = await _httpClient.GetStringAsync($"api/Blog/getById/{id}");
            var obj = JsonConvert.DeserializeObject<Blog>(datajson);
            return View(obj);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, Blog obj, IFormFile file)
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
            var jsonData = JsonConvert.SerializeObject(obj);
            HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("api/Blog/update", content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return BadRequest(response.Content.ReadAsStringAsync());
        }


        public async Task<IActionResult> Delete(Guid id)
        {

            var response = await _httpClient.DeleteAsync($"api/Blog/delete/{id}");
            return RedirectToAction("Index");
        }
        
    }
}
