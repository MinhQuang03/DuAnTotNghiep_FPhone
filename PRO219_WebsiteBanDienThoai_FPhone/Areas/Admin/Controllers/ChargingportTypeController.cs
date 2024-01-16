using AppData.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Filters;
using System.Text;

namespace PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ChargingportTypeController : Controller
    {
        public readonly HttpClient _httpClient;
        public ChargingportTypeController(HttpClient httpClient)
        {
            _httpClient = httpClient;

        }
       
        public async Task<IActionResult> Index()
        {
            var datajson = await _httpClient.GetStringAsync("api/ChargingportType/get");
            var obj = JsonConvert.DeserializeObject<List<ChargingportType>>(datajson);
            return View(obj);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ChargingportType obj)
        {
            if (!ModelState.IsValid)
            {
                return View(obj);
            }
            try
            {
                var jsonData = JsonConvert.SerializeObject(obj);
                HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("api/ChargingportType/add", content);
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

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var datajson = await _httpClient.GetStringAsync($"api/ChargingportType/getById/{id}");
            var obj = JsonConvert.DeserializeObject<ChargingportType>(datajson);
            return View(obj);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, ChargingportType obj)
        {
            if (!ModelState.IsValid)
            {
                return View(obj);
            }
            var jsonData = JsonConvert.SerializeObject(obj);

            HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync("api/ChargingportType/update", content);
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Delete(Guid id)
        {
            
            var response = await _httpClient.DeleteAsync($"api/ChargingportType/delete/{id}");
            return RedirectToAction("Index");
        }
    }
}
