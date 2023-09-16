﻿using AppData.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OperatingController : Controller
    {
        public readonly HttpClient _httpClient;
        public OperatingController(HttpClient httpClient)
        {
            _httpClient = httpClient;

        }

        public async Task<IActionResult> Index()
        {
            var datajson = await _httpClient.GetStringAsync("api/Operating/get");
            var obj = JsonConvert.DeserializeObject<List<OperatingSystems>>(datajson);
            return View(obj);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(OperatingSystems obj)
        {
            try
            {
                var jsonData = JsonConvert.SerializeObject(obj);
                HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("api/Operating/add", content);
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
            var datajson = await _httpClient.GetStringAsync($"api/Operating/getById/{id}");
            var obj = JsonConvert.DeserializeObject<OperatingSystems>(datajson);
            return View(obj);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, OperatingSystems obj)
        {
            var jsonData = JsonConvert.SerializeObject(obj);

            HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync("api/Operating/update", content);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(Guid id)
        {

            var response = await _httpClient.DeleteAsync($"api/Operating/delete/{id}");
            return RedirectToAction("Index");
        }
    }
}
