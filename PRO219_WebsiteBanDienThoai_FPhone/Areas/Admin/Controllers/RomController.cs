﻿using AppData.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RomController : Controller
    {
        public readonly HttpClient _httpClient;
        public RomController(HttpClient httpClient)
        {
            _httpClient = httpClient;

        }
        public async Task<IActionResult> Index()
        {
            var datajson = await _httpClient.GetStringAsync("api/Rom/get");
            var obj = JsonConvert.DeserializeObject<List<Rom>>(datajson);
            return View(obj);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Rom obj)
        {
            try
            {
                var jsonData = JsonConvert.SerializeObject(obj);
                HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("api/Rom/add", content);
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
            var datajson = await _httpClient.GetStringAsync($"api/Rom/getById/{id}");
            var obj = JsonConvert.DeserializeObject<Rom>(datajson);
            return View(obj);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, Rom obj)
        {
            var jsonData = JsonConvert.SerializeObject(obj);

            HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync("api/Rom/update", content);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(Guid id)
        {

            var response = await _httpClient.DeleteAsync($"api/Rom/delete/{id}");
            return RedirectToAction("Index");
        }

    }
}
