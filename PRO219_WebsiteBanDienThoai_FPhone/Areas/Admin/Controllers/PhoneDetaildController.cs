﻿using AppData.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PhoneDetaildController : Controller
    {
        public readonly HttpClient _httpClient;
        public PhoneDetaildController(HttpClient httpClient)
        {

            _httpClient = httpClient;

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

            foreach (var a in obj)
            {
                if (!phoneName.ContainsKey(a.IdPhone))
                {
                    var phoneNameData = await _httpClient.GetStringAsync($"api/Phone/getById/{a.IdPhone}");
                    var phone = JsonConvert.DeserializeObject<Phone>(phoneNameData);
                    phoneName.Add(a.IdPhone, phone.PhoneName);
                }

                if (!materialName.ContainsKey(a.IdMaterial))
                {
                    var materialNameData = await _httpClient.GetStringAsync($"api/Phone/getById/{a.IdMaterial}");
                    var material = JsonConvert.DeserializeObject<Material>(materialNameData);
                    materialName.Add(a.IdMaterial, material.Name);
                }

                if (!ramName.ContainsKey(a.IdRam))
                {
                    var ramNameData = await _httpClient.GetStringAsync($"api/Phone/getById/{a.IdRam}");
                    var ram = JsonConvert.DeserializeObject<Ram>(ramNameData);
                    ramName.Add(a.IdRam, ram.Name);
                }

                if (!romName.ContainsKey(a.IdRom))
                {
                    var romNameData = await _httpClient.GetStringAsync($"api/Phone/getById/{a.IdRom}");
                    var rom = JsonConvert.DeserializeObject<Rom>(romNameData);
                    romName.Add(a.IdRom, rom.Name);
                }

                if (!OperatingSystemName.ContainsKey(a.IdOperatingSystem))
                {
                    var OperatingSystemNameData = await _httpClient.GetStringAsync($"api/Phone/getById/{a.IdOperatingSystem}");
                    var OperatingSystem = JsonConvert.DeserializeObject<OperatingSystems>(OperatingSystemNameData);
                    OperatingSystemName.Add(a.IdOperatingSystem, OperatingSystem.Name);
                }

                if (!batteryName.ContainsKey(a.IdBattery))
                {
                    var batteryNameData = await _httpClient.GetStringAsync($"api/Phone/getById/{a.IdBattery}");
                    var battery = JsonConvert.DeserializeObject<Battery>(batteryNameData);
                    batteryName.Add(a.IdBattery, battery.Name);
                }

                if (!simName.ContainsKey(a.IdSim))
                {
                    var simNameData = await _httpClient.GetStringAsync($"api/Phone/getById/{a.IdSim}");
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

            return View(obj);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PhoneDetaild obj)
        {
            try
            {
                var jsonData = JsonConvert.SerializeObject(obj);
                HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("api/PhoneDetaild/add", content);
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
            var datajson = await _httpClient.GetStringAsync($"api/PhoneDetaild/getById/{id}");
            var obj = JsonConvert.DeserializeObject<PhoneDetaild>(datajson);
            return View(obj);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, PhoneDetaild obj)
        {
            var jsonData = JsonConvert.SerializeObject(obj);

            HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync("api/PhoneDetaild/update", content);
            return RedirectToAction("Index");
        }

    }
}
