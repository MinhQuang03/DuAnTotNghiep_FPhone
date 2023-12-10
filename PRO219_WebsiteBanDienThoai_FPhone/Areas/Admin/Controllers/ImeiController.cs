using AppData.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Filters;
using System.Text;

namespace PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AuthenFilter]
    public class ImeiController : Controller
    {
        public readonly HttpClient _httpClient;
        public ImeiController(HttpClient httpClient)
        {

            _httpClient = httpClient;

        }
        public async Task<IActionResult> Index()
        {
            var datajson = await _httpClient.GetStringAsync("api/Imei/get");
            var obj = JsonConvert.DeserializeObject<List<Imei>>(datajson);

            var materialName = new Dictionary<Guid, string>();
            var ramName = new Dictionary<Guid, string>();
            var phoneName = new Dictionary<Guid, string>();

            foreach (var imei in obj)
            {
                var phoneDetail = await _httpClient.GetStringAsync($"api/PhoneDetaild/getById/{imei.IdPhoneDetaild}");
                var phoneDetailObj = JsonConvert.DeserializeObject<PhoneDetaild>(phoneDetail);

                // Lấy thông tin từ bảng Material
                var material = await _httpClient.GetStringAsync($"api/Material/getById/{phoneDetailObj.IdMaterial}");
                var materialObj = JsonConvert.DeserializeObject<Material>(material);
                materialName[phoneDetailObj.IdMaterial] = materialObj.Name;

                // Lấy thông tin từ bảng Ram
                var ram = await _httpClient.GetStringAsync($"api/Ram/getById/{phoneDetailObj.IdRam}");
                var ramObj = JsonConvert.DeserializeObject<Ram>(ram);
                ramName[phoneDetailObj.IdRam] = ramObj.Name;

                // Lấy thông tin từ bảng phone
                var phone = await _httpClient.GetStringAsync($"api/Phone/getById/{phoneDetailObj.IdPhone}");
                var phoneObj = JsonConvert.DeserializeObject<Phone>(phone);
                phoneName[phoneDetailObj.IdPhone] = phoneObj.PhoneName;

                ViewBag.InformationPhone = phoneObj.PhoneName + " " + materialObj.Name + " " + ramObj.Name;
            }

            ViewBag.MaterialName = materialName;
            ViewBag.RamName = ramName;
            ViewBag.PhoneName = phoneName;

            return View(obj);
        }
        public async Task<IActionResult> Create()
        {
            var phoneNameData = await _httpClient.GetStringAsync("api/PhoneDetaild/get");
            List<PhoneDetaild> phone = JsonConvert.DeserializeObject<List<PhoneDetaild>>(phoneNameData);
            ViewBag.IdPhone = new SelectList(phone, "Id", "IdPhone");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Imei obj)
        {
            try
            {
                var jsonData = JsonConvert.SerializeObject(obj);
                HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("api/Imei/add", content);
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
            var phoneNameData = await _httpClient.GetStringAsync("api/PhoneDetaild/get");
            List<PhoneDetaild> phone = JsonConvert.DeserializeObject<List<PhoneDetaild>>(phoneNameData);
            ViewBag.IdPhone = new SelectList(phone, "Id", "IdPhone");

            var datajson = await _httpClient.GetStringAsync($"api/Imei/getById/{id}");
            var obj = JsonConvert.DeserializeObject<Imei>(datajson);
            return View(obj);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, Imei obj)
        {
            var jsonData = JsonConvert.SerializeObject(obj);
            HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("api/Imei/update", content);
            return RedirectToAction("Index");
        }



    }
}
