using AppData.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Filters;
using System.Text;

namespace PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AuthenFilter]
    public class MaterialController : Controller
    {
        public readonly HttpClient _httpClient;
        public MaterialController(HttpClient httpClient)
        {
            _httpClient = httpClient;

        }

        // GET: MaterialController
        public async Task<IActionResult> Index()
        {
            var datajson = await _httpClient.GetStringAsync("api/material/get");
            var obj = JsonConvert.DeserializeObject<List<Material>>(datajson);
            return View(obj);
        }

        // GET: MaterialController/Details/5
        public IActionResult Create()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Create(Material obj)
        {
            if (!ModelState.IsValid)
            {
                return View(obj);
            }
            try
            {
                var jsonData = JsonConvert.SerializeObject(obj);
                HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("api/material/add", content);
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Them thanh cong";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }

            return View();
        }
        // GET: MaterialController/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var datajson = await _httpClient.GetStringAsync($"api/material/getById/{id}");
            var obj = JsonConvert.DeserializeObject<Material>(datajson);
            return View(obj);
        }

        // POST: MaterialController/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, Material obj)
        {
            if (!ModelState.IsValid)
            {
                return View(obj);
            }
            var jsonData = JsonConvert.SerializeObject(obj);

            HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync("api/material/update", content);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(Guid id)
        {

            var response = await _httpClient.DeleteAsync($"api/material/delete/{id}");
            return RedirectToAction("Index");
        }
    }
}
