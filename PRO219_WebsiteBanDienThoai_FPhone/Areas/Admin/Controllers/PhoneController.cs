using AppData.FPhoneDbContexts;
using AppData.IRepositories;
using AppData.IServices;
using AppData.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Filters;
using PRO219_WebsiteBanDienThoai_FPhone.ViewModel;
using System.Text;
using System.Collections.Generic;
using System.IO;
using AppData.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;


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
        public FPhoneDbContext _dbContext;

        public PhoneController(HttpClient httpClient, IVwPhoneService service, IVwPhoneDetailService detailService, IListImageService imageService, IPhoneRepository phoneRepository)
        {
            _httpClient = httpClient;
            _service = service;
            _detailService = detailService;
            _imageService = imageService;
            _phoneRepository = phoneRepository;
            _dbContext = new FPhoneDbContext();
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
            AdPhoneInsertViewModel model = new AdPhoneInsertViewModel();
            model.ListWarranty = _service.ListWarrty();
            model.ListCompany = _service.ListCompany();
            return View(model);
        }


        public IActionResult ListPhoneDetail(Guid id)
        {
            AdPhoneDetailViewModel model = new AdPhoneDetailViewModel();
            model.IDPhone = id;
            model.SearchData.IdPhone = id;
            model.ListVwPhoneDetail = _detailService.listVwPhoneDetails(model.SearchData, model.ListOptions).Where(c => c.IdPhone == id).ToList();
            foreach (var item in model.ListVwPhoneDetail)
            {
                item.FirstImage = _imageService.GetFirstImageByIdPhondDetail(item.IdPhoneDetail);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult ListPhoneDetail(AdPhoneDetailViewModel model)
        {
            model.ListVwPhoneDetail = _detailService.listVwPhoneDetails(model.SearchData, model.ListOptions).Where(c => c.IdPhone == model.SearchData.IdPhone).ToList();

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(AdPhoneInsertViewModel obj, IFormFile file)
        {
            obj.ListWarranty = _service.ListWarrty();
            obj.ListCompany = _service.ListCompany();

            if (_phoneRepository.CheckExitPhone(obj.PhoneName)>0)
            {
                ModelState.AddModelError("PhoneName", "Tên sản phẩm này đã tồn tại");
                return View(obj);
            }

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

            Phone data = new Phone()
            {
                Id = obj.Id,
                PhoneName = obj.PhoneName,
                CreateDate = obj.CreateDate,
                Description = obj.Description,
                IdProductionCompany = obj.IdProductionCompany,
                IdWarranty = obj.IdWarranty,
                Image = obj.Image
            };
            BeanUtils.CopyAllPropertySameName(obj,data);
            var jsonData = JsonConvert.SerializeObject(data);
            HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/Phone/add", content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View(jsonData);
        }



        public IActionResult ListImeiPhoneDetail(Guid IdPhoneDetail)
        {
            var imei = _dbContext.Imei.Where(p => p.IdPhoneDetaild == IdPhoneDetail && p.Status == 1).ToList();

            var phoneDetail = _dbContext.PhoneDetailds.FirstOrDefault(p => p.Id == IdPhoneDetail);
            var ram = _dbContext.Ram.FirstOrDefault(p => p.Id == phoneDetail.IdRam);
            var color = _dbContext.Colors.FirstOrDefault(p => p.Id == phoneDetail.IdColor);
            var phoneName = _dbContext.Phones.FirstOrDefault(p => p.Id == phoneDetail.IdPhone);

            ImeiPhoneViewModel model = new ImeiPhoneViewModel();
            model.IdPhoneDetail = IdPhoneDetail;
            model.imeis = imei;
            model.PhoneDetailName = phoneName.PhoneName + " " + ram.Name + " " + color.Name;

           

            return View(model);
        }


        public async Task<IActionResult> CreateImei(Guid IdPhoneDetail)
        {
            var phoneDetail = _dbContext.PhoneDetailds.FirstOrDefault(p => p.Id == IdPhoneDetail);
            var ram = _dbContext.Ram.FirstOrDefault(p => p.Id == phoneDetail.IdRam);
            var color = _dbContext.Colors.FirstOrDefault(p => p.Id == phoneDetail.IdColor);
            var phoneName = _dbContext.Phones.FirstOrDefault(p => p.Id == phoneDetail.IdPhone);

            ImeiPhoneViewModel model = new ImeiPhoneViewModel();
            model.IdPhoneDetail = IdPhoneDetail;
            model.PhoneDetailName = phoneName.PhoneName + " " + ram.Name + " " + color.Name;
            model.PhoneDetaild = phoneDetail;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateImei(ImeiPhoneViewModel obj)
        {
            var a = _dbContext.Imei.FirstOrDefault(p => p.NameImei == obj.AddImeiOfPhone.NameImei);
            if (obj.AddImeiOfPhone.NameImei.Length != 15)
            {
                TempData["ErrorMessage"] = "Độ dài của Imei phải là 15 ký tự!";
                return RedirectToAction("CreateImei", new { IdPhoneDetail = obj.PhoneDetaild.Id });
            }

                if (a != null)
                {
                    TempData["ErrorMessage"] = "Không được thêm trùng imei !";
                    return RedirectToAction("CreateImei", new { IdPhoneDetail = a.IdPhoneDetaild });
                }
                else
                {
                    Imei newImei = new Imei
                    {
                        Id = Guid.NewGuid(),
                        NameImei = obj.AddImeiOfPhone.NameImei,
                        IdPhoneDetaild = obj.PhoneDetaild.Id,
                        Status = 1
                    };


                    _dbContext.Imei.Add(newImei);
                    await _dbContext.SaveChangesAsync();
                    return RedirectToAction("ListImeiPhoneDetail", new { IdPhoneDetail = newImei.IdPhoneDetaild });
                }
            
            
        }



        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            AdPhoneDetailViewModel model = new AdPhoneDetailViewModel();
            model.ListCompany = _service.ListCompany();
            model.ListWarranty = _service.ListWarrty();
            model.PhoneDetail = await _phoneRepository.GetById(id);
            var data = await _httpClient.GetStringAsync("api/ProductionCompany/get");
            List<ProductionCompany> a = JsonConvert.DeserializeObject<List<ProductionCompany>>(data);
            ViewBag.IdProductionCompany = new SelectList(a, "Id", "Name");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AdPhoneDetailViewModel obj, IFormFile file)
        {
            obj.ListCompany = _service.ListCompany();
            obj.ListWarranty = _service.ListWarrty();



            if (file != null && file.Length > 0) // khong null va khong trong 
            {
                var fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                obj.PhoneDetail.Image = "/img/" + fileName;
            }

            Phone data = new Phone()
            {
                Id = obj.PhoneDetail.Id,
                PhoneName = obj.PhoneDetail.PhoneName,
                CreateDate = obj.PhoneDetail.CreateDate,
                Description = obj.PhoneDetail.Description,
                IdProductionCompany = obj.PhoneDetail.IdProductionCompany,
                IdWarranty = obj.PhoneDetail.IdWarranty,
                Image = obj.PhoneDetail.Image
            };
            var jsonData = JsonConvert.SerializeObject(data);
            HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("api/Phone/update", content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            //return BadRequest(response.Content.ReadAsStringAsync());
            return View(obj);
        }

        #region Nhập imei từ file excle
        [HttpPost]
        public async Task<IActionResult> Import(List<IFormFile> files)
        {
            try
            {
                int successCount = 0;
                int errorCount = 0;

                List<Imei> listImei = new List<Imei>();
                Guid IdPhoneDetaild = Guid.Parse("bf7c3bcd-1357-4969-bc62-0d595993bad0");

                if (files != null && files.Any())
                {
                    foreach (var file in files)
                    {
                        if (file.Length > 0)
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                file.CopyTo(memoryStream);
                                using (var package = new ExcelPackage(memoryStream))
                                {
                                    // Khai báo biến cục bộ
                                    string nameImei;
                                    string idPhoneDetaild;

                                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                                    // Process each row in the Excel file
                                    for (int row = 2; row <= worksheet.Dimension.Rows; row++)
                                    {
                                        nameImei = worksheet.Cells[row, 2].Text;
                                        idPhoneDetaild = worksheet.Cells[row, 3].Text;

                                        // Kiểm tra IdPhoneDetaild có tồn  tại trong hệ thống hay không?
                                        var checkPhone = _dbContext.PhoneDetailds.FirstOrDefault(n => n.Id == Guid.Parse(idPhoneDetaild));

                                        // Kiểm tra Imei từ excel có tồn tại trong hệ thống hay chưa (Vì ImeiName là duy nhất)
                                        var checkImei = _dbContext.Imei.FirstOrDefault(n => n.NameImei == nameImei);

                                        if (checkPhone != null && checkImei == null) // Trường hợp tồn tại đt trong hệ thống
                                        {
                                            IdPhoneDetaild = checkPhone.Id;
                                            Imei imei = new Imei
                                            {
                                                NameImei = nameImei,
                                                Status = 1,
                                                IdPhoneDetaild = Guid.Parse(idPhoneDetaild)
                                            };

                                            successCount++;

                                            _dbContext.Imei.Add(imei);
                                            _dbContext.SaveChanges();
                                        }
                                        // Trường hợp không tồn tại phone trong hệ thống hoặc đã tồn tại email trong hệ thống rồi
                                        else
                                        {
                                            listImei.Add(
                                                new Imei
                                                {
                                                    NameImei = nameImei,
                                                    IdPhoneDetaild = Guid.Parse(idPhoneDetaild)
                                                }
                                                );

                                            errorCount++;
                                        }
                                    }
                                }
                            }
                        }
                    }

                }

                TempData["SuccessMessage"] = $"Bạn đã thêm thành công {successCount} dòng và có {errorCount} dòng gặp lỗi.";

                // Hiển thị ra danh sách lỗi
                //ViewBag.Imei = listImei;
                //TempData["Imei"] = listImei.ToList();
                return RedirectToAction("ListImeiPhoneDetail", new { IdPhoneDetail = IdPhoneDetaild });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        // thông báo input thành công
        //public IActionResult ThongBao()
        //{
        //    AdPhoneDetailViewModel model = new AdPhoneDetailViewModel();
        //    model.IDPhone = id;
        //    model.SearchData.IdPhone = id;
        //    model.ListVwPhoneDetail = _detailService.listVwPhoneDetails(model.SearchData, model.ListOptions).Where(c => c.IdPhone == id).ToList();
        //    foreach (var item in model.ListVwPhoneDetail)
        //    {
        //        item.FirstImage = _imageService.GetFirstImageByIdPhondDetail(item.IdPhoneDetail);
        //    }
        //    return View(model);
        //}
        #endregion

        // Xuất file 
        public IActionResult Export()
        {
            try
            {
                List<Imei> listImei = _dbContext.Imei.ToList(); // Lấy danh sách Imei từ cơ sở dữ liệu

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("ImeiData");

                    // Add header row
                    worksheet.Cells["A1"].Value = "NameImei";
                    worksheet.Cells["B1"].Value = "Phone Name";
                    worksheet.Cells["C1"].Value = "Status";

                    // Add data rows
                    int row = 2;
                    foreach (var imei in listImei)
                    {

                        var phoneDetail = _dbContext.PhoneDetailds.FirstOrDefault(p => p.Id == imei.IdPhoneDetaild);
                        var ram = _dbContext.Ram.FirstOrDefault(p => p.Id == phoneDetail.IdRam);
                        var color = _dbContext.Colors.FirstOrDefault(p => p.Id == phoneDetail.IdColor);
                        var phoneName = _dbContext.Phones.FirstOrDefault(p => p.Id == phoneDetail.IdPhone);

                        var tb = "";
                        if(imei.Status == 1)
                        {
                            tb = "Còn hàng";
                        }
                        else
                        {
                            tb = "Đã bán";
                        }

                        worksheet.Cells[$"A{row}"].Value = imei.NameImei;
                        worksheet.Cells[$"B{row}"].Value = phoneName.PhoneName + " " + ram.Name + " " + color.Name;
                        worksheet.Cells[$"C{row}"].Value = tb;

                        row++;
                    }

                    // Set the file name and content type
                    string fileName = "ImeiData.xlsx";
                    string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                    // Save the Excel package to a memory stream
                    var stream = new MemoryStream(package.GetAsByteArray());

                    TempData["SuccessMessage"] = "Bạn đã export thành công";

                    // Return the Excel file as a FileStreamResult
                    return File(stream, contentType, fileName);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                // Handle the exception as needed
                return null;
            }
        }


    }
}
