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
                                 .Include(p => p.Accounts)
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
        public async Task<IActionResult> SendEmail(string toEmail, DateTime? dNgayGuiBaohanh)
        {
            try
            {
                // Thông tin tài khoản email của bạn
                string fromEmail = "fphone.store.404@gmail.com";
                string password = "bdrczcwdttczwbsv";

                var acc = _context.Accounts.FirstOrDefault(p => p.Email == toEmail);

                // Tạo đối tượng MailMessage
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(fromEmail);
                mailMessage.To.Add(toEmail);
                mailMessage.Subject = "Bảo Hành Điện Thoại";
                mailMessage.Body = $@"
        <!DOCTYPE html>
        <html lang='en'>
        <head>
            <meta charset='UTF-8'>
            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
            <style>
                body {{
                    font-family: 'Arial', sans-serif;
                    background-color: #f4f4f4;
                    margin: 0;
                    padding: 0;
                }}
                .container {{
                    max-width: 600px;
                    margin: 20px auto;
                    padding: 20px;
                    border: 1px solid #ddd;
                    border-radius: 8px;
                    background-color: #fff;
                    box-shadow: 0 0 10px rgba(0,0,0,0.1);
                }}
                h1 {{
                    color: #007BFF;
                    margin-bottom: 20px;
                }}
                p {{
                    margin-bottom: 15px;
                    line-height: 1.6;
                    color: #555;
                }}
                strong {{
                    font-weight: bold;
                }}
                ul {{
                    list-style: none;
                    padding: 0;
                }}
                li {{
                    margin-bottom: 8px;
                }}
                a {{
                    color: #007BFF;
                    text-decoration: none;
                    font-weight: bold;
                }}
                footer {{
                    margin-top: 20px;
                    text-align: center;
                    color: #777;
                }}
            </style>
        </head>
        <body>
            <div class='container'>
                <h1>{acc.Username} thân mến,</h1>
                <p>Chúng tôi rất vui thông báo rằng yêu của quý khách đã được xem xét và đồng ý bởi <strong> FPHONE STORE. </strong></p>
                <p>Thông tin bảo hành như sau:</p>
                <p>Hãy gửi tới sau đây: 123ACCC<p>
                <p>Thời gian bảo hành sẽ tính đến ngày: {dNgayGuiBaohanh.Value.AddMonths(3)}, kể từ ngày bạn gửi yêu cầu bảo hành.<p>
                <p>Nếu Quý khách có bất kỳ câu hỏi hoặc cần hỗ trợ, vui lòng liên hệ với chúng tôi qua email <a href='mailto:support@fphonestore.com'>support@fphonestore.com</a> hoặc gọi số điện thoại hỗ trợ khách hàng: <strong>0123-456-789</strong>.</p>
                <p>Chúc Quý khách có những trải nghiệm tốt nhất với hệ thống của chúng tôi!</p>
                <footer>
                    Trân trọng,<br>
                    FPHONE STORE
                </footer>
            </div>
        </body>
        </html>";

                // Cấu hình đối tượng SmtpClient
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
                smtpClient.Port = 587;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(fromEmail, password);
                smtpClient.EnableSsl = true;

                // Gửi email
                await smtpClient.SendMailAsync(mailMessage);

                return Ok("Email sent successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error sending email: {ex.Message}");
            }
        }
    }
}
