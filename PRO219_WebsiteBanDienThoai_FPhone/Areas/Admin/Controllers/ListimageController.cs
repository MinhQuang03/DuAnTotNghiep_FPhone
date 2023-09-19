using AppData.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ListimageController : Controller
    {
        public readonly HttpClient _httpClient;
        public ListimageController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IActionResult> Index()
        {
            var datajson = await _httpClient.GetStringAsync("api/listimage/get");
            
            var image = JsonConvert.DeserializeObject<List<ListImage>>(datajson);
            

            var phoneName = new Dictionary<Guid, string>();
            var phoneNamedetaild = new Dictionary<Guid, string>();
            

            foreach (var phone in image)
            {
                if (!phoneName.ContainsKey(phone.IdColor.Value))
                {
                    var productionCompanyData = await _httpClient.GetStringAsync($"api/Color/getById/{phone.IdColor}");
                    var productionCompany = JsonConvert.DeserializeObject<Color>(productionCompanyData);
                    phoneName.Add(phone.IdColor.Value, productionCompany.Name);
                }

                if (!phoneNamedetaild.ContainsKey((Guid)phone.IdPhoneDetaild))
                {
                    var productionCompanyData = await _httpClient.GetStringAsync($"api/PhoneDetaild/getById/{phone.IdPhoneDetaild}");
                    var phoneName1 = JsonConvert.DeserializeObject<Phone>(productionCompanyData);
                    phoneNamedetaild.Add((Guid)phone.IdPhoneDetaild, phoneName1.PhoneName);
                }

            }
            ViewBag.color = phoneName;
            ViewBag.PhoneDeltaild = phoneNamedetaild;

            return View(image);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ListImage obj)
        {
            try
            {
                var jsonData = JsonConvert.SerializeObject(obj);
                HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("api/Listimage/add", content);
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
            var datajson = await _httpClient.GetStringAsync($"api/Listimage/getById/{id}");
            var obj = JsonConvert.DeserializeObject<ListImage>(datajson);
            return View(obj);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, ListImage obj)
        {
            var jsonData = JsonConvert.SerializeObject(obj);

            HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync("api/Listimage/update", content);
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Delete(Guid id)
        {

            var response = await _httpClient.DeleteAsync($"api/Listimage/delete/{id}");
            return RedirectToAction("Index");
        }
    }
}
