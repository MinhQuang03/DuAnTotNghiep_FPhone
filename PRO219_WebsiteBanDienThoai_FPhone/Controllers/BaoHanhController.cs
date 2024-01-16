using AppData.FPhoneDbContexts;
using AppData.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PRO219_WebsiteBanDienThoai_FPhone.Controllers
{
    public class BaoHanhController : Controller
    {
        private FPhoneDbContext _context;
        public BaoHanhController()
        {
            _context = new FPhoneDbContext();
        }
        public IActionResult Index()
        {
            return View();
        }

        public ActionResult XemChiTiet(Guid idBill)
        {
            var billDetail = _context.BillDetails
                            .Include(p => p.PhoneDetaild)
                                .ThenInclude(p => p.Phones)
                            .Include(p => p.PhoneDetaild)
                                .ThenInclude(p => p.Colors)
                            .Include(p => p.PhoneDetaild)
                                .ThenInclude(p => p.Rams)
                            .Include(p => p.PhoneDetaild)
                                .ThenInclude(p => p.Roms)
                            .Where(p => p.IdBill == idBill)
                            .ToList();

            // Get Status of table Bill
            var billStatus = _context.Bill.Find(idBill);
            ViewBag.BillStatus = billStatus.Status;
            return View(billDetail);
        }

        public ActionResult ThongTinBaoHanh(Guid idBillDetail)
        {
            var phone = _context.BillDetails
                            .Include(p => p.PhoneDetaild)
                                .ThenInclude(p => p.Phones)
                            .Include(p => p.PhoneDetaild)
                                .ThenInclude(p => p.Colors)
                            .Include(p => p.PhoneDetaild)
                                .ThenInclude(p => p.Rams)
                            .Include(p => p.PhoneDetaild)
                                .ThenInclude(p => p.Roms)
                            .Include(p => p.Bills)
                                .ThenInclude(p => p.Accounts)
                        .Where(p => p.Id == idBillDetail).ToList();
            return View(phone);
        }



        // Yêu cầu bảo hành
        [HttpPost]
        public ActionResult YeuCauBaoHanh(Guid IdPhoneDetail, string phoneImei, string note)
        {
            var billDetail = _context.BillDetails.Include(a => a.Bills)
                                                 .ThenInclude(a => a.Accounts)
                                                 .Include(a => a.PhoneDetaild)
                                                 .SingleOrDefault(a => a.IdPhoneDetail == IdPhoneDetail && a.Imei == phoneImei);

            if (null != billDetail)
            {
                //billDetail.Status = 4; // Yêu cầu bảo hành 
                //_context.SaveChanges();

                var check = _context.WarrantyCards.FirstOrDefault(p => p.IdBillDetail == billDetail.Id);
                if(check == null)
                {
                    var warrantyCard = new WarrantyCard();
                    warrantyCard.Id = Guid.NewGuid();
                    warrantyCard.IdBillDetail = billDetail.Id;
                    warrantyCard.IdAccount = billDetail.Bills.IdAccount;
                    warrantyCard.IdPhoneDetail = IdPhoneDetail;
                    warrantyCard.IdPhone = billDetail.PhoneDetaild.IdPhone;
                    warrantyCard.Imei = phoneImei;
                    warrantyCard.CreatedDate = DateTime.Now;
                    warrantyCard.Description = note; // Có thể thay đổi tùy theo yêu cầu
                                                     //ThoiGianConBaoHanh = billDetail.Bills.PaymentDate.AddMonths(billDetail.PhoneDetaild.Phones.IdWarranty.TimeWarranty),
                    warrantyCard.Status = 0; // 1: Mới tạo
                                             // Bổ sung thêm các thông tin khác nếu cần
                    warrantyCard.ThoiGianConBaoHanh = DateTime.Now;

                    TempData["SuccessMessage"] = "Bạn đã gửi yêu cầu thành công!";
                    _context.WarrantyCards.Add(warrantyCard);
                    _context.SaveChanges();
                }
                else
                {
                    TempData["ErrorMessage"] = "Máy của bạn đang được bảo hành. Không thể gửi thêm yêu cầu!";
                }
                
            }
            return RedirectToAction("XemChiTiet", new { idBill = billDetail.IdBill });
        }
    }
}
