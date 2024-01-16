using AppData.FPhoneDbContexts;
using AppData.IServices;
using AppData.Models;
using AppData.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Web.Providers.Entities;
using static AppData.Utilities.Trangthai;

namespace PRO219_WebsiteBanDienThoai_FPhone.Controllers
{
  
        
    public class DiscountController : Controller
    {
        public readonly FPhoneDbContext _context;
        public readonly IDiscountServices _voucherSV;
       
        public readonly User user;
        public DiscountController(IDiscountServices discountServices , FPhoneDbContext fPhoneDb)
        {
            _context = fPhoneDb;
            _voucherSV = discountServices;
          
        }
        public async Task<IActionResult> Index()
        {
            var idNguoiDung = User.Claims.FirstOrDefault(claim => claim.Type == "Id")?.Value;
            if (string.IsNullOrEmpty(idNguoiDung))
            {
                ViewBag.NguoiDung = null;
            }
            else ViewBag.NguoiDung = idNguoiDung;
            return View();
        }
        public async Task<IActionResult> VoucherLstToCalm(string LoaiHinh)
        {
            var idNguoiDung = User.Claims.FirstOrDefault(claim => claim.Type == "Id")?.Value;
            if (string.IsNullOrEmpty(idNguoiDung))
            {
                ViewBag.NguoiDung = null;
            }
            else ViewBag.NguoiDung = idNguoiDung;

            var allVouchers = (await _voucherSV.GetallVoucher()).Where(c => c.StatusVoucher == "0" && c.Quantity > 0);
            switch (LoaiHinh)
            {
                case "TienMat":
                    allVouchers = allVouchers.Where(c => c.TypeVoucher == (int)TrangThaiLoaiKhuyenMai.TienMat);
                    break;
                case "PhanTram":
                    allVouchers = allVouchers.Where(c => c.TypeVoucher == (int)TrangThaiLoaiKhuyenMai.PhanTram);
                    break;
                case "FreeShip":
                    allVouchers = allVouchers.Where(c => c.TypeVoucher == (int)TrangThaiLoaiKhuyenMai.Freeship);
                    break;
                default:
                    break;
            }
            ViewBag.TatCaVoucher = allVouchers;
            return PartialView("_VoucherLstToCalm", allVouchers);
        }
        //public async Task<IActionResult> GetVoucherById(string idVoucher)
        //{
        //    var Voucher = await _voucherSV.get(idVoucher);
        //    double mucuidai = 0;
        //    string IdVoucher = "";
        //    int loaiuudai = 0;
        //    if (Voucher != null)
        //    {
        //        mucuidai = (double)Voucher.MucUuDai;
        //        IdVoucher = Voucher.IdVoucher;
        //        loaiuudai = (int)Voucher.LoaiHinhUuDai;
        //    }
        //    return Json(new { mucuidai, IdVoucher, loaiuudai });
        //}
        //public async Task<IActionResult> UpdateVoucherAfterUseIt(string idVoucher)
        //{
        //    var idNguoiDung = _userManager.GetUserId(User);
        //    if (await _voucherSV.UpdateVoucherAfterUseIt(idVoucher, idNguoiDung))
        //    {
        //        return Ok();
        //    }
        //    return BadRequest();
        //}
        //public async Task<IActionResult> UpdateVoucherSoluong(string idVoucher)
        //{
        //    if (await _voucherSV.UpdateVoucherSoluong(idVoucher))
        //    {
        //        return Ok();
        //    }
        //    return BadRequest();
        //}
        // GET: DiscountController

    }
}
