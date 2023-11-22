using AppData.FPhoneDbContexts;
using Microsoft.AspNetCore.Mvc;

namespace PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BillController : Controller
    {
        private FPhoneDbContext _context;
        public BillController()
        {
                _context = new FPhoneDbContext();
        }
        public IActionResult Index()
        {
            var Bill = _context.Bill.ToList();
            return View(Bill);
        }
    }
}
