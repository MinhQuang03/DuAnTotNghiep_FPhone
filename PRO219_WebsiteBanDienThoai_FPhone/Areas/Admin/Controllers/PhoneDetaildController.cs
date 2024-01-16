using AppData.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Filters;
using System.Text;
using AppData.IServices;
using AppData.Utilities;
using AppData.ViewModels;
using PRO219_WebsiteBanDienThoai_FPhone.ViewModel;

namespace PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AuthenFilter]
    public class PhoneDetaildController : Controller
    {
        public readonly HttpClient _httpClient;
        private IVwPhoneDetailService _service;
        private IListImageService _imageService;
       
        public PhoneDetaildController(HttpClient httpClient,IVwPhoneDetailService service, IListImageService imageService)
        {
            _httpClient = httpClient;
            _service = service;
            _imageService = imageService;
        }
        public async Task<IActionResult> Index()
        {
            var datajson = await _httpClient.GetStringAsync("api/PhoneDetaild/get");
            var obj = JsonConvert.DeserializeObject<List<PhoneDetaild>>(datajson);

            var phoneName = new Dictionary<Guid, string>();
            var materialName = new Dictionary<Guid, string>();
            var ramName = new Dictionary<Guid, string>();
            var romName = new Dictionary<Guid, string>();
            var OperatingSystemName = new Dictionary<Guid, string>();
            var batteryName = new Dictionary<Guid, string>();
            var simName = new Dictionary<Guid, string>();
            var chipCPUName = new Dictionary<Guid, string>();
            var chipGPUName = new Dictionary<Guid, string>();
            var colorName = new Dictionary<Guid, string>();
            var chargingportName = new Dictionary<Guid, string>();

            var productionCompanyNames = new Dictionary<Guid, string>();

            foreach (var a in obj)
            {
                if (!productionCompanyNames.ContainsKey(a.Phones.IdProductionCompany))
                {
                    var productionCompanyData = await _httpClient.GetStringAsync($"api/ProductionCompany/getById/{a.Phones.IdProductionCompany}");
                    var productionCompany = JsonConvert.DeserializeObject<ProductionCompany>(productionCompanyData);
                    productionCompanyNames.Add(a.Phones.IdProductionCompany, productionCompany.Name);
                }
                if (!phoneName.ContainsKey(a.IdPhone))
                {
                    var phoneNameData = await _httpClient.GetStringAsync($"api/Phone/getById/{a.IdPhone}");
                    var phone = JsonConvert.DeserializeObject<Phone>(phoneNameData);
                    phoneName.Add(a.IdPhone, phone.PhoneName);
                }

                if (!materialName.ContainsKey(a.IdMaterial))
                {
                    var materialNameData = await _httpClient.GetStringAsync($"api/Material/getById/{a.IdMaterial}");
                    var material = JsonConvert.DeserializeObject<Material>(materialNameData);
                    materialName.Add(a.IdMaterial, material.Name);
                }

                if (!ramName.ContainsKey(a.IdRam))
                {
                    var ramNameData = await _httpClient.GetStringAsync($"api/Ram/getById/{a.IdRam}");
                    var ram = JsonConvert.DeserializeObject<Ram>(ramNameData);
                    ramName.Add(a.IdRam, ram.Name);
                }

                if (!romName.ContainsKey(a.IdRom))
                {
                    var romNameData = await _httpClient.GetStringAsync($"api/Rom/getById/{a.IdRom}");
                    var rom = JsonConvert.DeserializeObject<Rom>(romNameData);
                    romName.Add(a.IdRom, rom.Name);
                }

                if (!OperatingSystemName.ContainsKey(a.IdOperatingSystem))
                {
                    var OperatingSystemNameData = await _httpClient.GetStringAsync($"api/Operating/getById/{a.IdOperatingSystem}");
                    var OperatingSystem = JsonConvert.DeserializeObject<OperatingSystems>(OperatingSystemNameData);
                    OperatingSystemName.Add(a.IdOperatingSystem, OperatingSystem.Name);
                }

                if (!batteryName.ContainsKey(a.IdBattery))
                {
                    var batteryNameData = await _httpClient.GetStringAsync($"api/Battery/getById/{a.IdBattery}");
                    var battery = JsonConvert.DeserializeObject<Battery>(batteryNameData);
                    batteryName.Add(a.IdBattery, battery.Name);
                }

                if (!simName.ContainsKey(a.IdSim))
                {
                    var simNameData = await _httpClient.GetStringAsync($"api/Sim/getById/{a.IdSim}");
                    var sim = JsonConvert.DeserializeObject<Sim>(simNameData);
                    simName.Add(a.IdSim, sim.Name);
                }

                if (!chipCPUName.ContainsKey(a.IdChipCPU))
                {
                    var chipCPUNameData = await _httpClient.GetStringAsync($"api/ChipCPUs/getById/{a.IdChipCPU}");
                    var chipCPU = JsonConvert.DeserializeObject<ChipCPUs>(chipCPUNameData);
                    chipCPUName.Add(a.IdChipCPU, chipCPU.Name);
                }

                if (!chipGPUName.ContainsKey(a.IdChipGPU))
                {
                    var chipGPUNameData = await _httpClient.GetStringAsync($"api/ChipGPUs/getById/{a.IdChipGPU}");
                    var chipGPU = JsonConvert.DeserializeObject<ChipCPUs>(chipGPUNameData);
                    chipGPUName.Add(a.IdChipGPU, chipGPU.Name);
                }

                if (!colorName.ContainsKey(a.IdColor))
                {
                    var colorNameData = await _httpClient.GetStringAsync($"api/Colors/getById/{a.IdColor}");
                    var color = JsonConvert.DeserializeObject<Color>(colorNameData);
                    colorName.Add(a.IdColor, color.Name);
                }

                if (!chargingportName.ContainsKey(a.IdChargingport))
                {
                    var chargingportNameData = await _httpClient.GetStringAsync($"api/ChargingportType/getById/{a.IdChargingport}");
                    var chargingport = JsonConvert.DeserializeObject<ChargingportType>(chargingportNameData);
                    chargingportName.Add(a.IdChargingport, chargingport.Name);
                }
            }

            ViewBag.PhoneName = phoneName;
            ViewBag.MaterialName = materialName;
            ViewBag.RamName = ramName;
            ViewBag.RomName = romName;
            ViewBag.OperatingSystemName = OperatingSystemName;
            ViewBag.BatteryName = batteryName;
            ViewBag.SimName = simName;
            ViewBag.ChipCPUName = chipCPUName;
            ViewBag.ChipGPUName = chipGPUName;
            ViewBag.ColorName = colorName;
            ViewBag.ChargingportName = chargingportName;

            ViewBag.ProductionCompanyNames = productionCompanyNames;

            return View(obj);
        }

        public async Task<IActionResult> Create(string idphone) 
        {
            AdPhoneDetailViewModel model = new AdPhoneDetailViewModel();
            var phonedetail = await _httpClient.GetStringAsync($"api/Phone/getById/{idphone}");
            if (phonedetail != null)
            {
                model.PhoneDetail = JsonConvert.DeserializeObject<Phone>(phonedetail);
            }
            var phoneNameData = await _httpClient.GetStringAsync("api/Phone/get");
            List<Phone> phone = JsonConvert.DeserializeObject<List<Phone>>(phoneNameData);
            ViewBag.IdPhone = new SelectList(phone, "Id", "PhoneName");

            var materialNameData = await _httpClient.GetStringAsync("api/Material/get");
            List<Material> material = JsonConvert.DeserializeObject<List<Material>>(materialNameData);
            ViewBag.IdMaterial = new SelectList(material, "Id", "Name");

            var ramName = await _httpClient.GetStringAsync("api/Ram/get");
            List<Ram> ram = JsonConvert.DeserializeObject<List<Ram>>(ramName);
            ViewBag.IdRam = new SelectList(ram, "Id", "Name");

            var romName = await _httpClient.GetStringAsync("api/Rom/get");
            List<Rom> rom = JsonConvert.DeserializeObject<List<Rom>>(romName);
            ViewBag.IdRom = new SelectList(rom, "Id", "Name");

            var OperatingSystemName = await _httpClient.GetStringAsync("api/Operating/get");
            List<OperatingSystems> OperatingSystem = JsonConvert.DeserializeObject<List<OperatingSystems>>(OperatingSystemName);
            ViewBag.IdOperatingSystems = new SelectList(OperatingSystem, "Id", "Name");

            var batteryName = await _httpClient.GetStringAsync("api/Battery/get");
            List<Battery> battery = JsonConvert.DeserializeObject<List<Battery>>(batteryName);
            ViewBag.IdBattery = new SelectList(battery, "Id", "Name");

            var simName = await _httpClient.GetStringAsync("api/Sim/get");
            List<Sim> sim = JsonConvert.DeserializeObject<List<Sim>>(simName);
            ViewBag.IdSim = new SelectList(sim, "Id", "Name");

            var chipCPUName = await _httpClient.GetStringAsync("api/ChipCPUs/get");
            List<ChipCPUs> chipCPU = JsonConvert.DeserializeObject<List<ChipCPUs>>(chipCPUName);
            ViewBag.IdChipCPUs = new SelectList(chipCPU, "Id", "Name");

            var chipGPUName = await _httpClient.GetStringAsync("api/ChipGPUs/get");
            List<ChipGPUs> chipGPU = JsonConvert.DeserializeObject<List<ChipGPUs>>(chipGPUName);
            ViewBag.IdChipGPUs = new SelectList(chipGPU, "Id", "Name");

            var colorName = await _httpClient.GetStringAsync("api/Colors/get");
            List<Color> color = JsonConvert.DeserializeObject<List<Color>>(colorName);
            ViewBag.IdColor = new SelectList(color, "Id", "Name");

            var chargingportName = await _httpClient.GetStringAsync("api/ChargingportType/get");
            List<ChargingportType> chargingport = JsonConvert.DeserializeObject<List<ChargingportType>>(chargingportName);
            ViewBag.IdChargingportType = new SelectList(chargingport, "Id", "Name");

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AdPhoneDetailViewModel obj)
        {
            DataError er = new DataError() { Success = true };
            PhoneDetaild model = new PhoneDetaild();
            model = obj.DetailOfPhoneDetaild;
            model.IdPhone = obj.PhoneDetail.Id;
            model.Id = Guid.NewGuid();
            model.Code = Utility.RandomString(6);
            try
            {
              var result =  _service.Add(model);
                
                if (result!=null)
                {
                    er.Msg = "Thêm thành công";
                    return Redirect($"/Admin/Phone/ListPhoneDetail/{result.IdPhone}");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }

            return View(obj);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var phoneNameData = await _httpClient.GetStringAsync("api/Phone/get");
            List<Phone> phone = JsonConvert.DeserializeObject<List<Phone>>(phoneNameData);
            ViewBag.IdPhone = new SelectList(phone, "Id", "PhoneName");

            var materialNameData = await _httpClient.GetStringAsync("api/Material/get");
            List<Material> material = JsonConvert.DeserializeObject<List<Material>>(materialNameData);
            ViewBag.IdMaterial = new SelectList(material, "Id", "Name");

            var ramName = await _httpClient.GetStringAsync("api/Ram/get");
            List<Ram> ram = JsonConvert.DeserializeObject<List<Ram>>(ramName);
            ViewBag.IdRam = new SelectList(ram, "Id", "Name");

            var romName = await _httpClient.GetStringAsync("api/Rom/get");
            List<Rom> rom = JsonConvert.DeserializeObject<List<Rom>>(romName);
            ViewBag.IdRom = new SelectList(rom, "Id", "Name");

            var OperatingSystemName = await _httpClient.GetStringAsync("api/Operating/get");
            List<OperatingSystems> OperatingSystem = JsonConvert.DeserializeObject<List<OperatingSystems>>(OperatingSystemName);
            ViewBag.IdOperatingSystems = new SelectList(OperatingSystem, "Id", "Name");

            var batteryName = await _httpClient.GetStringAsync("api/Battery/get");
            List<Battery> battery = JsonConvert.DeserializeObject<List<Battery>>(batteryName);
            ViewBag.IdBattery = new SelectList(battery, "Id", "Name");

            var simName = await _httpClient.GetStringAsync("api/Sim/get");
            List<Sim> sim = JsonConvert.DeserializeObject<List<Sim>>(simName);
            ViewBag.IdSim = new SelectList(sim, "Id", "Name");

            var chipCPUName = await _httpClient.GetStringAsync("api/ChipCPUs/get");
            List<ChipCPUs> chipCPU = JsonConvert.DeserializeObject<List<ChipCPUs>>(chipCPUName);
            ViewBag.IdChipCPUs = new SelectList(chipCPU, "Id", "Name");

            var chipGPUName = await _httpClient.GetStringAsync("api/ChipGPUs/get");
            List<ChipGPUs> chipGPU = JsonConvert.DeserializeObject<List<ChipGPUs>>(chipGPUName);
            ViewBag.IdChipGPUs = new SelectList(chipGPU, "Id", "Name");

            var colorName = await _httpClient.GetStringAsync("api/Colors/get");
            List<Color> color = JsonConvert.DeserializeObject<List<Color>>(colorName);
            ViewBag.IdColor = new SelectList(color, "Id", "Name");

            var chargingportName = await _httpClient.GetStringAsync("api/ChargingportType/get");
            List<ChargingportType> chargingport = JsonConvert.DeserializeObject<List<ChargingportType>>(chargingportName);
            ViewBag.IdChargingportType = new SelectList(chargingport, "Id", "Name");


            var datajson = await _httpClient.GetStringAsync($"api/PhoneDetaild/getById/{id}");
            var obj = JsonConvert.DeserializeObject<PhoneDetaild>(datajson);
            return View(obj);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, PhoneDetaild obj)
        {
            //var jsonData = JsonConvert.SerializeObject(obj);

            //HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            //var response = await _httpClient.PutAsync("api/PhoneDetaild/update", content);
          var data =   _service.Update(obj);
          if (data!=null)
          {
              return RedirectToAction("Index","Phone");
            }

          return View(obj);
        }

        public IActionResult ViewDetail(Guid id)    
        {
            AdPhoneDetailViewModel model = new AdPhoneDetailViewModel(); 
            model.VwPhoneDetail =  _service.getPhoneDetailByIdPhoneDetail(id);
            model.VwPhoneDetail.FirstImage = _imageService.GetFirstImageByIdPhondDetail(id);
            return View(model.VwPhoneDetail);
        }

    }
}
