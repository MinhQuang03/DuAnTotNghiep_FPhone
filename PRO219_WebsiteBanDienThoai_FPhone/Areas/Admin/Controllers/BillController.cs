using AppData.FPhoneDbContexts;
using AppData.Models;
using Microsoft.AspNetCore.Mvc;
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

            // Lấy danh sách các PhoneName và gán vào ViewBag
            ViewBag.PhoneNames = phoneNames;

            ViewBag.customer = _context.Bill.Where(m => m.Id == id).First();
            var lisst = _context.BillDetails.Where(m => m.IdBill == id).ToList();
            return View("BillDetail", lisst);
        }

    }
}
