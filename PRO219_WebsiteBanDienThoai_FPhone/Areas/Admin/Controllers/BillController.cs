using AppData.FPhoneDbContexts;
using Microsoft.AspNetCore.Mvc;
using PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Filters;

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

      
    }
}
