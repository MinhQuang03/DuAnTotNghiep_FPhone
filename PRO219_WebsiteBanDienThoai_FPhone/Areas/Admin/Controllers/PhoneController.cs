using AppData.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Filters;
using System.Text;
using AppData.IRepositories;
using AppData.IServices;
using AppData.Services;
using PRO219_WebsiteBanDienThoai_FPhone.ViewModel;

namespace PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AuthenFilter]
    public class PhoneController : Controller
    {
        public readonly HttpClient _httpClient;
        private IVwPhoneService _service;
        private IVwPhoneDetailService _detailService;
        private IListImageService _imageService;
        private IPhoneRepository _phoneRepository;

        public PhoneController(HttpClient httpClient, IVwPhoneService service, IVwPhoneDetailService detailService, IListImageService imageService, IPhoneRepository phoneRepository)
        {
            _httpClient = httpClient;
            _service = service;
            _detailService = detailService;
            _imageService = imageService;
            _phoneRepository = phoneRepository;
        }
        public async Task<IActionResult> Index()
        {
            AdPhoneViewModel model = new AdPhoneViewModel();
            model.ListVwPhoneGroup = _service.listVwPhoneGroup(model.SearchData, model.ListOptions);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Index(AdPhoneViewModel model)
        {
            model.ListVwPhoneGroup = _service.listVwPhoneGroup(model.SearchData, model.ListOptions);
            return View(model);
        }
        public async Task<IActionResult> Create()
        {
            //var datajson = await _httpClient.GetStringAsync("api/ProductionCompany/get");
            //List<ProductionCompany> obj = JsonConvert.DeserializeObject<List<ProductionCompany>>(datajson);
            //ViewBag.IdProductionCompany = new SelectList(obj, "Id","Name");
            return View();
        }


        public IActionResult ListPhoneDetail(Guid id)
        {
            AdPhoneDetailViewModel model = new AdPhoneDetailViewModel();
            model.SearchData.IdPhone = id;
            model.ListVwPhoneDetail = _detailService.listVwPhoneDetails(model.SearchData,model.ListOptions).Where(c =>c.IdPhone == id).ToList();
            foreach (var item in model.ListVwPhoneDetail)
            {
                item.FirstImage = _imageService.GetFirstImageByIdPhondDetail(item.IdPhoneDetail);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult ListPhoneDetail(AdPhoneDetailViewModel model)
        {
            model.ListVwPhoneDetail = _detailService.listVwPhoneDetails(model.SearchData, model.ListOptions).Where(c => c.IdPhone == model.SearchData.IdPhone).ToList(); ;
            return View(model);
        }
        [HttpPost] 
        public async Task<IActionResult> Create(Phone obj, IFormFile file)
        {
            if (file != null && file.Length > 0) // khong null va khong trong 
            {
                var fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                obj.Image = "/img/" + fileName;
            }
            var jsonData = JsonConvert.SerializeObject(obj);
            HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/Phone/add", content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View(jsonData);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            AdPhoneDetailViewModel model = new AdPhoneDetailViewModel();
            model.PhoneDetail = await _phoneRepository.GetById(id);
            var data = await _httpClient.GetStringAsync("api/ProductionCompany/get");
            List<ProductionCompany> a = JsonConvert.DeserializeObject<List<ProductionCompany>>(data);
            ViewBag.IdProductionCompany = new SelectList(a, "Id", "Name");

            //var datajson = await _httpClient.GetStringAsync($"api/Phone/getById/{id}");
            //var obj = JsonConvert.DeserializeObject<Phone>(datajson);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AdPhoneDetailViewModel model)
        {
            //if (file != null && file.Length > 0) // khong null va khong trong 
            //{
            //    var fileName = Path.GetFileName(file.FileName);
            //    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);
            //    using (var stream = new FileStream(filePath, FileMode.Create))
            //    {
            //        file.CopyTo(stream);
            //    }

            //    obj.Image = "/img/" + fileName;
            //}
            //var jsonData = JsonConvert.SerializeObject(obj);
            //HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            //var response = await _httpClient.PutAsync("api/Phone/update", content);
            //if (response.IsSuccessStatusCode)
            //{
            //    return RedirectToAction("Index");
            //}
            //return BadRequest(response.Content.ReadAsStringAsync());
            return View(model);
        }
    }
}
