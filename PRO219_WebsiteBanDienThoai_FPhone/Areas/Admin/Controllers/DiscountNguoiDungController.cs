using AppData.FPhoneDbContexts;
using AppData.IServices;
using AppData.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PRO219_WebsiteBanDienThoai_FPhone.IServices;

namespace PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Controllers
{
    public class DiscountNguoiDungController : Controller
    {

        public readonly FPhoneDbContext _context;
        public readonly IVoucherNguoiDung _voucherND;
        private readonly SignInManager<Account> _signInManager;
        private readonly UserManager<Account> _userManager;
        private readonly AppData.IServices.IDiscountServices _voucherSV;
        // GET: VoucherNguoiDungController

        public DiscountNguoiDungController(IVoucherNguoiDung voucherNguoiDung,SignInManager<Account> signInManager , UserManager<Account> userManger , AppData.IServices.IDiscountServices discountServices )
        {
            _context = new FPhoneDbContext();
            _voucherND = voucherNguoiDung;
            _signInManager = signInManager;
            _userManager = userManger;
            _voucherSV = discountServices;
        }
        public ActionResult Index()
        {
            return View();
        }

        // GET: VoucherNguoiDungController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: VoucherNguoiDungController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VoucherNguoiDungController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: VoucherNguoiDungController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: VoucherNguoiDungController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: VoucherNguoiDungController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: VoucherNguoiDungController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
