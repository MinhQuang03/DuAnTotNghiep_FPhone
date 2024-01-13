using AppData.FPhoneDbContexts;
using AppData.IRepositories;
using AppData.IServices;
using AppData.Utilities;
using Microsoft.AspNetCore.Mvc;
using PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Filters;
using PRO219_WebsiteBanDienThoai_FPhone.ViewModel;

namespace PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AuthenFilter]

    public class AdCheckOutController : Controller
    {
       
        private IProductionCompanyRepository _companyRepository;
        private IRamRepository _ramRepository;
        private IChipCPURepository _chipCpuRepository;
        private IRomRepository _romRepository;
        private IMaterialRepository _materialRepository;
        private IVwPhoneDetailService _phoneDetailService;
        private IListImageService _imageService;
        private IAccountService _accountService;
        private IBillRepository _billRepository;

        public AdCheckOutController( IProductionCompanyRepository companyRepository, IRamRepository ramRepository, IChipCPURepository chipCpuRepository, IRomRepository romRepository, IMaterialRepository materialRepository, IVwPhoneDetailService phoneDetailService, IListImageService imageService, IAccountService accountService, IBillRepository billRepository)
        {
            _companyRepository = companyRepository;
            _ramRepository = ramRepository;
            _chipCpuRepository = chipCpuRepository;
            _romRepository = romRepository;
            _materialRepository = materialRepository;
            _phoneDetailService = phoneDetailService;
            _imageService = imageService;
            _accountService = accountService;
            _billRepository = billRepository;
        }
        public async Task<IActionResult> Index()
        {
            AdCheckOutViewModel model = new AdCheckOutViewModel();
            model.ListvVwPhoneDetails = _phoneDetailService.listVwPhoneDetails(null).Where(c =>c.Status == FphoneConst.HoatDong).ToList();
            //Gán ảnh cho sản phẩm(avatar)
            foreach (var item in model.ListvVwPhoneDetails)
            {
                //lấy ra ảnh đầu tiên trong list ảnh
                item.FirstImage = _imageService.GetFirstImageByIdPhondDetail(item.IdPhoneDetail) == ""
                    ? " "
                    : _imageService.GetFirstImageByIdPhondDetail(item.IdPhoneDetail);
                //đếm số lượng sản phẩm
                item.CountPhone = _phoneDetailService.CountPhoneDetailFromImei(item.IdPhoneDetail);
               
            }
            return View(model);
        }
        [HttpGet("/AdCheckOut/GetPhoneDetail/{id}")]
        public JsonResult GetPhoneDetail(string id)
        {
            var dbo = _phoneDetailService.getPhoneDetailByIdPhoneDetail(Guid.Parse(id));
            return Json(dbo);
        }
        [HttpGet("/AdCheckOut/GetUserByPhone/{phoneNumber}")]
        public JsonResult GetUserByPhone(string phoneNumber)
        {
            var dbo = _accountService.GetUserByPhoneNumber(phoneNumber);
            if (dbo==null)
            {
                return Json("");
            }
            return Json(dbo);
        }

        [HttpPost]
        public async Task AdCheckOut(AdCheckOutViewModel model) 
        {   
            model.Bill.PaymentDate = DateTime.Now;
            model.Bill.Status = FphoneConst.DaGiao;
            model.Bill.StatusPayment = FphoneConst.DaThanhToan;
            var bill = await  _billRepository.Add(model.Bill);
            if (bill!=null)
            {
                foreach (var item in model.ListBillDetail)
                {
                    item.IdBill = model.Bill.Id;
                    item.Number = 1;
                    item.Price = _phoneDetailService.getPhoneDetailByIdPhoneDetail(item.IdPhoneDetail).Price == null
                        ? 0
                        : _phoneDetailService.getPhoneDetailByIdPhoneDetail(item.IdPhoneDetail).Price.Value;
                    item.Status = FphoneConst.HoanThanh;
                    _billRepository.AddBillDetail(item);
                }
            }
        }

        [HttpPost("/AdCheckOut/CheckOutJSon")]
        public JsonResult CheckOutJSon(AdCheckOutViewModel model)
        {
            model.Bill.PaymentDate = DateTime.Now;
            model.Bill.Status = FphoneConst.ChoXacNhan;
            model.Bill.StatusPayment = FphoneConst.DaThanhToan;
            model.Bill.Address = "Bán hàng tại quầy";
            var bill =  _billRepository.Add(model.Bill).Result;
            if (bill != null)
            {
                foreach (var item in model.ListBillDetail)
                {
                    item.IdBill = model.Bill.Id;
                    item.Number = 1;
                    item.Price = _phoneDetailService.getPhoneDetailByIdPhoneDetail(item.IdPhoneDetail).Price == null
                        ? 0
                        : _phoneDetailService.getPhoneDetailByIdPhoneDetail(item.IdPhoneDetail).Price.Value;
                    item.Status = FphoneConst.HoanThanh;
                    _billRepository.AddBillDetail(item);
                }

                return Json("Thanh toán thành công");
            }
            return Json("Thanh toán không thành công. Vui lòng thử lại");
        }

       
    }
}
