using AppData.FPhoneDbContexts;
using AppData.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Filters;
using System.Net;
using System.Net.Mail;

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
            // Lấy danh sách hóa đơn giảm dần theo thời gian đặt hàng
            var bills = _context.Bill.Where(b => b.Status != 0).ToList().OrderByDescending(b => b.CreatedTime);
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
            if (bill.Status == 2)
            {
                //Loii
            }
            else
            {
                bill.Status = 3;
                _context.Entry(bill).State = EntityState.Modified;
                _context.SaveChanges();

                var soldImeis = (from billDetail in _context.BillDetails
                                 join imei in _context.Imei on billDetail.IdPhoneDetail equals imei.IdPhoneDetaild
                                 where billDetail.IdBill == id && imei.Status == 1
                                 select imei).ToList();
                var bd = _context.BillDetails.FirstOrDefault(bd => bd.IdBill == id);

                if (soldImeis.Any())
                {
                    // 1 = chua ban, 2 = da ban
                    var imeiToGet = soldImeis.FirstOrDefault();
                    imeiToGet.Status = 2;
                    imeiToGet.IdBillDetail = bd.Id;
                    _context.Entry(imeiToGet).State = EntityState.Modified;
                    _context.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }
        // thay đổi trạng thái thành đã giao
        public ActionResult Dagiao(Guid id)
        {
            Bill bill = _context.Bill.Find(id);
            if (bill.Status == 2)
            {
                // loi
            }
            else
            {

                bill.Status = 4;
                bill.StatusPayment = 1;
                _context.Entry(bill).State = EntityState.Modified;
                _context.SaveChanges();
            }

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

        // Hiển thị danh sách sản phẩm hoàn trả
        public ActionResult HoanTraHoacBaoHanh()
        {
            // List sản phẩm hoàn trả trong billDetail
            var billDetail = _context.BillDetails
                .Include(p => p.PhoneDetaild)
                    .ThenInclude(p => p.Phones)
                .Include(p => p.PhoneDetaild)
                    .ThenInclude(p => p.Colors)
                .Include(p => p.PhoneDetaild)
                    .ThenInclude(p => p.Rams)
                .Include(p => p.PhoneDetaild)
                    .ThenInclude(p => p.Roms)
                .Where(p => p.Status != 0)
                .ToList();

            return View(billDetail);
        }

        // Chấp nhận trả hàng
        public ActionResult ChapNhanHoanTra(Guid IdPhoneDetail, string phoneImei)
        {
            // Cập nhật trạng thái trong bảng BillDetail status = 2 (2: Chấp nhận trả hàng)
            var billDetail = _context.BillDetails.SingleOrDefault(a => a.IdPhoneDetail == IdPhoneDetail && a.Imei == phoneImei);
            if (null != billDetail)
            {
                billDetail.Status = 2;
                _context.SaveChanges();
            }

            return RedirectToAction("HoanTraHoacBaoHanh");
        }


        // Hủy trả hàng/Hủy
        public ActionResult HuyBaoHanhVaTraHang(Guid IdPhoneDetail, string phoneImei)
        {
            // Cập nhật trạng thái trong bảng BillDetail status = 2 (2: Chấp nhận trả hàng)
            var billDetail = _context.BillDetails.SingleOrDefault(a => a.IdPhoneDetail == IdPhoneDetail && a.Imei == phoneImei);
            if (null != billDetail)
            {
                billDetail.Status = 0;
                _context.SaveChanges();
            }

            return RedirectToAction("HoanTraHoacBaoHanh");
        }

        // Xác nhận đã nhận hàng gửi trả từ khách hàng
        public ActionResult TraHangThanhCong(Guid IdPhoneDetail, string phoneImei)
        {
            // Cập nhật trạng thái trong bảng BillDetail status = 3 (3: Đã trả hàng thành công)
            var billDetail = _context.BillDetails.SingleOrDefault(a => a.IdPhoneDetail == IdPhoneDetail && a.Imei == phoneImei); // Trả về một đối tượng
            if (null != billDetail)
            {
                billDetail.Status = 3;
                _context.SaveChanges();

                // Trường hợp bill có hoàn trả hết sản phẩm. Thì cập nhật lại Status trong bill = 6 (6: Đơn hoàn trả)
                // Lấy ra Bill chứa BillDetails
                var bill = _context.Bill.SingleOrDefault(a => a.Id == billDetail.IdBill);
                if (null != bill)
                {
                    // Kiểm tra trong bảng BillDetail có còn đơn hàng với trạng thái đã giao không
                    var checkBillDetail = _context.BillDetails.Where(a => a.IdBill == billDetail.IdBill & a.Status == 0).ToList(); //  Trả về danh sách
                    if (checkBillDetail.Count() == 0) // Đơn được trả hết
                    {
                        bill.Status = 6; // (6: Trả hàng)
                    }

                    billDetail.Status = 3;
                    _context.SaveChanges();
                }
            }


            // Cập nhật lại status=2 (chưa đượch bán) bảng Imei
            var imeil = _context.Imei.SingleOrDefault(a => a.IdPhoneDetaild == IdPhoneDetail && a.NameImei == phoneImei);
            if (null != imeil)
            {
                imeil.Status = 1;
                _context.SaveChanges();
            }

            return RedirectToAction("HoanTraHoacBaoHanh");
        }

        // Xác nhận bảo hành
        public ActionResult XacNhanBaoHanh(Guid IdPhoneDetail, string phoneImei)
        {
            // Cập nhật trạng thái trong bảng BillDetail status = 5 (5: Xác nhận bảo hành)
            var billDetail = _context.BillDetails.SingleOrDefault(a => a.IdPhoneDetail == IdPhoneDetail && a.Imei == phoneImei);
            if (null != billDetail)
            {
                billDetail.Status = 5;
                billDetail.Update_at = DateTime.Now;
                _context.SaveChanges();

                // Gửi mail thông báo cho khách hàng về thời gian bảo hành/ địa chỉ email gửi trả hàng
                // Tìm ra email theo thông tin khách hàng
                var billDetails = _context.Bill
                                 .Include(p => p.Accounts.Email)
                                 .FirstOrDefault(p => p.Id == billDetail.IdBill);

                SendEmail(billDetails.Accounts.Email, billDetail.Update_at);
            }

            return RedirectToAction("HoanTraHoacBaoHanh");
        }

        // Xác nhận bảo hành thành công (Máy đã được trả về cho khách hàng)
        public ActionResult BaoHanhThanhCong(Guid IdPhoneDetail, string phoneImei)
        {
            // Cập nhật trạng thái trong bảng BillDetail status = 0 (0: Trạng thái ban đầu) (1 máy có nhiều lần bảo hành)
            var billDetail = _context.BillDetails.SingleOrDefault(a => a.IdPhoneDetail == IdPhoneDetail && a.Imei == phoneImei);
            if (null != billDetail)
            {
                billDetail.Status = 0;
                _context.SaveChanges();
            }

            return RedirectToAction("HoanTraHoacBaoHanh");
        }

        // Gửi mail với nội dung bảo hành
        public void SendEmail(string toEmail, DateTime? dNgayGuiBaohanh)
        {
            // Địa chỉ email người gửi
            string fromEmail = "abc@gmail.com";
            string password = "122321423423524sdfsdfdsf";

            // Tạo đối tượng MailMessage
            MailMessage message = new MailMessage(fromEmail, toEmail);

            // Tiêu đề email
            message.Subject = "Bảo Hành Điện Thoại";

            // Nội dung email
            message.Body = "\nBạn vui lòng gửi đến địa chỉ: 123 Abc....\nXin cảm ơn\nNgày Gửi: " + dNgayGuiBaohanh
            + "Thời gian bảo hành đến: " + dNgayGuiBaohanh.Value.AddMonths(3);

            // Thiết lập một số tùy chọn khác nếu cần
            message.IsBodyHtml = true; // Nếu nội dung là HTML
            message.Priority = MailPriority.High; // Độ ưu tiên cao

            // Tạo đối tượng SmtpClient để gửi email
            SmtpClient smtpClient = new SmtpClient("smtp.example.com");

            // Thiết lập thông tin đăng nhập nếu máy chủ SMTP yêu cầu
            smtpClient.Credentials = new NetworkCredential(fromEmail, password);

            try
            {
                // Gửi email
                smtpClient.Send(message);
                ViewBag.Status = "Email đã được gửi thành công.";
            }
            catch (Exception ex)
            {
                ViewBag.Status = "Có lỗi khi gửi email: " + ex.Message;
            }

        }
    }
}
