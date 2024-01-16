using System.Text;
using AppData.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Filters;

namespace PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Controllers;

[Area("Admin")]
[AuthenFilter]
public class ColorController : Controller
{
    public readonly HttpClient _httpClient;

    public ColorController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }


    public async Task<IActionResult> Index()
    {
        var datajson = await _httpClient.GetStringAsync("api/Colors/get");
        var obj = JsonConvert.DeserializeObject<List<Color>>(datajson);
        return View(obj);
    }

    
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Color obj)
    {
        if (!ModelState.IsValid)
        {
            return View(obj);
        }
        try
        {
            var jsonData = JsonConvert.SerializeObject(obj);
            HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/Colors/add", content);
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
        var datajson = await _httpClient.GetStringAsync($"api/Colors/getById/{id}");
        var obj = JsonConvert.DeserializeObject<Color>(datajson);
        return View(obj);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Guid id, Color obj)
    {
        if (!ModelState.IsValid)
        {
            return View(obj);
        }
        var jsonData = JsonConvert.SerializeObject(obj);

        HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

        var response = await _httpClient.PutAsync("api/Colors/update", content);
        return RedirectToAction("Index");
    }


    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"api/Colors/delete/{id}");
        return RedirectToAction("Index");
    }
}