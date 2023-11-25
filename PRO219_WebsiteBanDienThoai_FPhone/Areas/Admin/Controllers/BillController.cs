using AppData.FPhoneDbContexts;
using AppData.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Filters;
using System.Net;

namespace PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AuthenFilter]
    public class BillController : Controller
    {
        private FPhoneDbContext _context;
        public BillController()
        {
                _context = new FPhoneDbContext();
        }
        public async Task<IActionResult> Index()
        {
            var bills = _context.Bill.ToList();
            if (bills != null && bills.Any())
            {
                return View(bills);
            }
            else
            {
                // Xử lý trường hợp không có hóa đơn
                return BadRequest("NoBills");
            }
        }
        public ActionResult Detail(Guid id)
        {
            if (id == null)
            {
                return BadRequest("NoDetaild");
            }
            var phoneNames = (from bd in _context.BillDetails
                             join pdp in _context.PhoneDetailds on bd.IdPhoneDetail equals pdp.Id
                             join ph in _context.Phones on pdp.IdPhone equals ph.Id
                             where bd.IdBill == id
                             select ph.PhoneName).FirstOrDefault();

            var ramName = (from bd in _context.BillDetails
                           join pdp in _context.PhoneDetailds on bd.IdPhoneDetail equals pdp.Id
                           join ph in _context.Ram on pdp.IdRam equals ph.Id
                           where bd.IdBill == id
                           select ph.Name).FirstOrDefault();

            var colorName = (from bd in _context.BillDetails
                           join pdp in _context.PhoneDetailds on bd.IdPhoneDetail equals pdp.Id
                           join ph in _context.Colors on pdp.IdColor equals ph.Id
                           where bd.IdBill == id
                           select ph.Name).FirstOrDefault();

            // Lấy danh sách các PhoneName và gán vào ViewBag
            ViewBag.PhoneNames = phoneNames + " " + ramName + " " + colorName;

            ViewBag.customer = _context.Bill.Where(m => m.Id == id).First();
            var lisst = _context.BillDetails.Where(m => m.IdBill == id).ToList();
            return View("BillDetail", lisst);
        }
        //chỉnh sửa status đơn hàng 
        // status = 1 đã xác nhận, 2 chờ xác nhận 
        public ActionResult Status(Guid id)
        {
            Bill bill = _context.Bill.Find(id);
            bill.Status = (bill.Status == 1) ? 2 : 1;
            _context.Entry(bill).State = EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // huỷ đơn hàng
        public ActionResult Dahuy(Guid id)
        {
            Bill bill = _context.Bill.Find(id);
            bill.Status = 5;    
            _context.Entry(bill).State = EntityState.Modified;
            _context.SaveChanges();
           
            return RedirectToAction("Index");
        }
        // đang giao hàng
        public ActionResult DangGiao(Guid id)
        {
            Bill bill = _context.Bill.Find(id);
            bill.Status = 3;
            _context.Entry(bill).State = EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        // thay đổi trạng thái thành đã giao
        public ActionResult Dagiao(Guid id)
        {
            Bill bill = _context.Bill.Find(id);
            bill.Status = 4;
            _context.Entry(bill).State = EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        // xoá đơn hàng , cập nhật lưu thay đổi
        public ActionResult Deltrash(Guid id)
        {
            Bill bill = _context.Bill.Find(id);
            bill.Status = 0;
            _context.Entry(bill).State = EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
