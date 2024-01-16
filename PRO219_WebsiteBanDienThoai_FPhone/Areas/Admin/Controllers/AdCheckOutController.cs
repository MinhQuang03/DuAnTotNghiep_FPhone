using AppData.IRepositories;
using AppData.IServices;
using AppData.Models;
using AppData.Services;
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
        private IVwPhoneDetailService _phoneDetailService;
        private IListImageService _imageService;
        private IAccountService _accountService;
        private IBillRepository _billRepository;
        private IEmailService _emailService;

        public AdCheckOutController(  IVwPhoneDetailService phoneDetailService, IListImageService imageService, IAccountService accountService, IBillRepository billRepository, IEmailService emailService)
        {
            _phoneDetailService = phoneDetailService;
            _imageService = imageService;
            _accountService = accountService;
            _billRepository = billRepository;
            _emailService = emailService;
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

        [HttpPost("/AdCheckOut/CreateAccountUser")]
        public JsonResult CreateAccountUser(AdCheckOutViewModel model)
        {
            Security security = new Security();
            string passrandom =  Utility.RandomString(6);
            var phone = _accountService.GetUserByPhoneNumber(model.Account.PhoneNumber);
            var email = _accountService.GetUserByEmail(model.Account.Email);
            var userName = _accountService.GetUserByUserName(model.Account.Username);
            if (userName!=null)
            {
                return Json("Tên đăng nhập này đã tồn tại");
            }
            if (email!=null)
            {
                return Json("Email này đã tồn tại");
            }
            if (phone == null)
            {
                model.Account.Status = FphoneConst.HoatDong;
                model.Account.Points = 0;
                model.Account.Password = security.Encrypt("B3C1035D5744220E", passrandom);
                var result = _accountService.CreateAccountForUser(model.Account);
                if (result != null)
                {
                    ObjectEmailInput emailInput = new ObjectEmailInput()
                    {
                        FullName = model.Account.Name,
                        SendTo = model.Account.Email,
                        Subject = "Thông báo tạo tài khoản",
                        Message = Utility.EmailCreateAccountTemplate(model.Account.Name,model.Account.Username, passrandom)
                    };
                     _emailService.SendEmail(emailInput); // gửi email
                    return Json("Tạo tài khoản thành công");
                }
            }
            else
            {
                return Json("Số điện thoại này đã tồn tại");
            }

            return Json("Tạo tài khoản không thành công");
        }
       
    }
}
