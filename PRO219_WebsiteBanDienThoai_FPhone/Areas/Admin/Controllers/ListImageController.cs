using AppData.IRepositories;
using AppData.IServices;
using AppData.Models;
using AppData.Utilities;
using AppData.ViewModels;
using Microsoft.AspNetCore.Mvc;
using PRO219_WebsiteBanDienThoai_FPhone.ViewModel;

namespace PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Controllers;
[Area("Admin")]
public class ListImageController : Controller
{
    private IVwPhoneDetailService _phoneDetailService;
    private IProductionCompanyRepository _companyRepository;
    private IRamRepository _ramRepository;
    private IChipCPURepository _chipCpuRepository;
    private IRomRepository _romRepository;
    private IMaterialRepository _materialRepository;
    private IListImageService _imageService;

    public ListImageController(IVwPhoneDetailService phoneDetailService, IProductionCompanyRepository companyRepository, IRamRepository ramRepository, IChipCPURepository chipCpuRepository, IRomRepository romRepository, IMaterialRepository materialRepository, IListImageService imageService)
    {
        _phoneDetailService = phoneDetailService;
        _companyRepository = companyRepository;
        _ramRepository = ramRepository;
        _chipCpuRepository = chipCpuRepository;
        _romRepository = romRepository;
        _materialRepository = materialRepository;
        _imageService = imageService;
    }

    public async Task<IActionResult> Index()
    {
        ListPhoneViewModel model = new ListPhoneViewModel();
        model.ListvVwPhoneDetails = _phoneDetailService.listVwPhoneDetails(model.SearchData, model.Options);
        model.Brand = await _companyRepository.GetAll();
        model.listRam = await _ramRepository.GetAll();
        model.listChipCPU = await _chipCpuRepository.GetAll();
        model.listRom = await _romRepository.GetAll();
        model.listMaterial = await _materialRepository.GetAll();
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> Index(ListPhoneViewModel model)
    {
        model.ListvVwPhoneDetails = _phoneDetailService.listVwPhoneDetails(model.SearchData, model.Options);
        model.Brand = await _companyRepository.GetAll();
        model.listRam = await _ramRepository.GetAll();
        model.listChipCPU = await _chipCpuRepository.GetAll();
        model.listRom = await _romRepository.GetAll();
        model.listMaterial = await _materialRepository.GetAll();
        return View(model);
    }

    public IActionResult InsertImageForPhoneDetail(Guid id)
    {
        ListPhoneViewModel model = new ListPhoneViewModel();
        model.Record = _phoneDetailService.getPhoneDetailByIdPhoneDetail(id);
        model.listImage = _imageService.GetListImagesByIdPhoneDetail(id);
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> InsertImageForPhoneDetail(ListPhoneViewModel model)
    {
        model.Record = _phoneDetailService.getPhoneDetailByIdPhoneDetail(model.Record.IdPhoneDetail);
        foreach (var item in model.Images)
        {
            if (item != null && item.Length > 0) // khong null va khong trong 
            {
                var fileName = Path.GetFileName(item.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await item.CopyToAsync(stream);
                }

                ListImage obj = new ListImage()
                {
                    Image = "/img/" + fileName,
                    IdPhoneDetaild = model.Record.IdPhoneDetail
                };
                //thực hiện thêm ảnh
                _imageService.Create(obj, out DataError error);
                if (error != null)
                {
                    TempData["DataError"] = Utility.ConvertObjectToJson(error);
                }
            }
        }

        return RedirectToAction("Index");
    }
}