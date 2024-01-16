using AppData.FPhoneDbContexts;
using AppData.IServices;
using AppData.Models;
using AppData.Utilities;
using AppData.ViewModels.Discount;
using AppData.ViewModels.DiscountNguoiDung;
using Castle.Core.Smtp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PRO219_WebsiteBanDienThoai_FPhone.IServices;
using PRO219_WebsiteBanDienThoai_FPhone.Services;
using System.Data.Entity;
using System.Text;
using System.Web.Providers.Entities;

namespace PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DiscountController : Controller
    {
        public readonly AppData.IServices.IDiscountServices _voucherSV;
        //public readonly UserManager<Account> _user;
        public readonly HttpClient _httpClient;
        public readonly FPhoneDbContext _phoneDbContext;
       
        //public readonly IVoucherNguoiDung _voucherND;
        //public readonly SignInManager<Account> _signInManager;
        private readonly IEmailSender _emailSender;
        private IViewRenderService _viewRenderService;
        public DiscountController(HttpClient httpClient)
        {
            _phoneDbContext = new FPhoneDbContext();
            _httpClient = httpClient;
            //_user = user;
            //_voucherND = voucherNguoiDung;
        }

        public async Task<IActionResult> Index(string trangthai)
        {
            var datajson = await _httpClient.GetStringAsync("api/Discount");
            var obj = JsonConvert.DeserializeObject<List<Discount>>(datajson);
            return View(obj);
        }
        [HttpPost]
        public async Task<IActionResult> FilterVoucherByStatus(string? trangThai)
        {
           
            var data = await _httpClient.GetStringAsync("/api/Discount");
            var lstdis = JsonConvert.DeserializeObject<List<Discount>>(data);
            if (trangThai != null)
            {
                lstdis = lstdis.Where(c => c.StatusVoucher == trangThai).OrderByDescending(c => c.DateStart).ToList();
            }

            return PartialView("_VoucherPartial", lstdis);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(DiscountDTO obj)
        {
            try
            {
                obj.MaVoucher = Utility.RandomString(5);
              
                var jsonData = JsonConvert.SerializeObject(obj);
                HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsJsonAsync("api/Discount/add", obj);
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
            var datajson = await _httpClient.GetStringAsync($"api/Discount/getById/{id}");
            var obj = JsonConvert.DeserializeObject<Discount>(datajson);
            return View(obj);
        }
        // GET: DiscountController/Details/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Discount obj)
        {
            var jsonData = JsonConvert.SerializeObject(obj);

            HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync("api/Discount/update", content);
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Delete(Guid id)
        {

            var response = await _httpClient.DeleteAsync($"api/Discount/delete/{id}");
            return RedirectToAction("Index");
        }
        //public async Task<IActionResult> VoucherLstToCalm(string LoaiHinh)
        //{
        //    var idNguoiDung = _user.GetUserId(User);
        //    if (string.IsNullOrEmpty(idNguoiDung))
        //    {
        //        ViewBag.NguoiDung = null;
        //    }
        //    else ViewBag.NguoiDung = idNguoiDung;

        //    var allVouchers = (await _voucherSV.GetallVoucher()).Where(c => c.StatusVoucher == 0 && c.Quantity > 0);
        //    switch (LoaiHinh)
        //    {
        //        case "TienMat":
        //            allVouchers = allVouchers.Where(c => c.TypeVoucher == (int)Trangthai.TrangThaiLoaiKhuyenMai.TienMat);
        //            break;
        //        case "PhanTram":
        //            allVouchers = allVouchers.Where(c => c.TypeVoucher == (int)Trangthai.TrangThaiLoaiKhuyenMai.PhanTram);
        //            break;
        //        case "FreeShip":
        //            allVouchers = allVouchers.Where(c => c.TypeVoucher == (int)Trangthai.TrangThaiLoaiKhuyenMai.Freeship);
        //            break;
        //        default:
        //            break;
        //    }
        //    ViewBag.TatCaVoucher = allVouchers;
        //    return PartialView("_VoucherLstToCalm", allVouchers);
        //}
        //public async Task<IActionResult> SendMail(string MaHd)
        //{
        //    var userKhachHang = "";
        //    if (SessionServices.GetIdFomSession(HttpContext.Session, "Email") != null)
        //    {
        //        userKhachHang = SessionServices.GetIdFomSession(HttpContext.Session, "Email");
        //    }
        //    else
        //    {
        //        var user = _userManager.GetUserId(User);
        //        userKhachHang = (await _userManager.FindByIdAsync(user)).Email;
        //    }
        //    var sp = SessionServices.GetFomSession(HttpContext.Session, "sp");
        //    var subject = $"Đơn hàng #{MaHd} đã đặt thành công";
        //    string html = await _viewRenderService.RenderToStringAsync("HoaDon/Mail", sp);
        //    await _emailSender.SendEmailAsync(userKhachHang, subject, html);
        //    return Ok();
        //}
     
        //public async Task<ActionResult> RestoreVoucherWithList(List<string> voucherIds)
        //{
        //    if (voucherIds.Any())
        //    {
        //        await _voucherSV.RestoreVoucherWithList(voucherIds);
        //        return Ok(true);
        //    }
        //    return Ok(false);
        //}
        //public async Task<ActionResult> GiveVouchersToUsers(string? maVoucher)
        //{
        //    ViewBag.MaVoucher = maVoucher;

        //    var lstUser = await _user.Users.ToListAsync();
        //    if (lstUser.Any())
        //    {
        //        ViewBag.User = lstUser;
        //    }

        //    return View();
        //}
        //[HttpPost]
        //public async Task<IActionResult> GiveVouchersToUsers([FromBody] AddDiscountRequestDTO addVoucherRequestDTO)
        //{
        //    if (addVoucherRequestDTO.MaVoucher == null)
        //    {
        //        return Ok(false);
        //    }
        //    if (addVoucherRequestDTO.UserId.Any())
        //    {
        //        var ketQuaThemVoucher = await _voucherND.AddVoucherNguoiDungTuAdmin(addVoucherRequestDTO);

        //        if (ketQuaThemVoucher == "Tặng voucher thành công")
        //        {
        //            foreach (var userId in addVoucherRequestDTO.UserId)
        //            {
        //                var userKhachHang = await _user.FindByIdAsync(userId);

        //                var subject = "Bạn đã nhận được Voucher từ Fphone";
        //                string html = await _viewRenderService.RenderToStringAsync("Discount/MailVoucher");

        //                await _emailSender.SendEmailAsync(userKhachHang.Email, subject, html);
        //            }

        //            return Ok(true);
        //        }

        //    }

        //    return Ok(false);
        //}
        //public async Task<IActionResult> GiveVoucherForNewUser(string MaVoucher)
        //{
        //    var ketQuaThemVoucher = await _voucherND.TangVoucherNguoiDungMoi(MaVoucher);
        //    if (ketQuaThemVoucher == true)
        //    {
        //        return Ok(true);
        //    }
        //    else
        //    {
        //        return Ok(false);
        //    }
        //}


    }
}
