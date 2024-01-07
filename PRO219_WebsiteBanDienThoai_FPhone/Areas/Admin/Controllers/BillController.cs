﻿using AppData.FPhoneDbContexts;
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
            var bills = _context.Bill.Where(b => b.Status == 0).ToList().OrderByDescending(b => b.CreatedTime);
            if (bills != null && bills.Any())
            {
                return View(bills);
            }
            else
            {
                return View();
            }
        }
        public async Task<IActionResult> xacnhan()
        {
            // Lấy danh sách hóa đơn giảm dần theo thời gian đặt hàng
            var bills = _context.Bill.Where(b => b.Status == 1).ToList().OrderByDescending(b => b.CreatedTime);
            if (bills != null && bills.Any())
            {
                return View(bills);
            }
            else
            {
                
                return View();
            }
        }
        public  ActionResult delete(Guid id)
        {

            var bills = _context.Bill.FirstOrDefault(b => b.Id == id);
            var bilsdetail = _context.BillDetails.Where(a=>a.IdBill == id).ToList();
            _context.BillDetails.RemoveRange(bilsdetail);
            _context.Bill.Remove(bills);
            _context.SaveChanges();

            return RedirectToAction("Dahuys");
        }

        public async Task<IActionResult> Dahuys()
        {
            // Lấy danh sách hóa đơn giảm dần theo thời gian đặt hàng
            var bills = _context.Bill.Where(b => b.Status == 4).ToList().OrderByDescending(b => b.CreatedTime);
            if (bills != null && bills.Any())
            {
                return View(bills);
            }
            else
            {
                return View();
            }
        }
        public async Task<IActionResult> Danggiaoview()
        {
            // Lấy danh sách hóa đơn giảm dần theo thời gian đặt hàng
            var bills = _context.Bill.Where(b => b.Status == 2).ToList().OrderByDescending(b => b.CreatedTime);
            if (bills != null && bills.Any())
            {
                return View(bills);
            }
            else
            {
                return View();
            }
        }
        public async Task<IActionResult> Dagiaoview()
        {
            // Lấy danh sách hóa đơn giảm dần theo thời gian đặt hàng
            var bills = _context.Bill.Where(b => b.Status == 3).ToList().OrderByDescending(b => b.CreatedTime);
            if (bills != null && bills.Any())
            {
                return View(bills);
            }
            else
            {
                return View();
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
            var lisst = _context.BillDetails.Where(m => m.IdBill == id && m.Status != 2).ToList();
            return View("BillDetail", lisst);
        }
        //chỉnh sửa status đơn hàng 
        // status = 0 cho xác nhận, 1 da xác nhận 
        public ActionResult Status(Guid id)
        {
            Bill bill = _context.Bill.Find(id);
            bill.Status = 1;
            _context.Entry(bill).State = EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // huỷ đơn hàng
        public ActionResult Dahuy(Guid id)
        {
            Bill bill = _context.Bill.Find(id);
            bill.Status = 4;

            var billdetail = _context.BillDetails.FirstOrDefault(p => p.IdBill == id);
            billdetail.Status = 2; // xoa

            _context.Entry(bill).State = EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        
        // đang giao hàng
        public ActionResult DangGiao(Guid id)
        {
            Bill bill = _context.Bill.Find(id);
            if (bill.Status == 0)
            {
                //Loii
            }
            else
            {
                bill.Status = 2;
                _context.Entry(bill).State = EntityState.Modified;
                _context.SaveChanges();  
            }
            return RedirectToAction("xacnhan");
        }
        // thay đổi trạng thái thành đã giao
        public ActionResult Dagiao(Guid id)
        {
            Bill bill = _context.Bill.Find(id);
            if (bill.Status == 0)
            {
                // loi
            }
            else
            {

                bill.Status = 3;
                bill.StatusPayment = 1;
                bill.PaymentDate = DateTime.Now;
                _context.Entry(bill).State = EntityState.Modified;
                _context.SaveChanges();
            }

            return RedirectToAction("Danggiaoview");
        }
        // xoá đơn hàng , cập nhật lưu thay đổi
        public ActionResult Deltrash(Guid id)
        {
            Bill bill = _context.Bill.Find(id);
            bill.Status = 5;
            _context.Entry(bill).State = EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        // Thêm mới imei 
        public ActionResult TimKiemImei(string name, Guid id)
        {
            
            var a = _context.Imei.FirstOrDefault(p => p.NameImei == name);
            if (a == null)
            {
                TempData["SuccessMessage"] = "Không tìm thấy sản phẩm !";
                return RedirectToAction("Detail", new { id = id });
            }
            else
            {
                
                
                var imei = _context.Imei.FirstOrDefault(p => p.NameImei == name);
                imei.Status = 2; // da ban

                var billdetaild = _context.BillDetails.FirstOrDefault(p => p.IdBill == id);
                billdetaild.Imei = name;

                billdetaild.Status = 1; // đã được nhân viên xác nhận bán, 0 là hủy hoặc chưa được xác nhận 

                _context.SaveChanges();
                TempData["SuccessMessage"] = "Đã thêm sản phẩm thành công !";


                return RedirectToAction("Detail", new { id = id});
            }
        }

        public ActionResult DeleteBilDetail(Guid id)
        {
            var a = _context.BillDetails.FirstOrDefault(p => p.Id == id);
            
            a.Status = 2; // xoa 
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Xóa sản phẩm thành công !";
            return RedirectToAction("Detail", new { id = a.IdBill });
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
                .Include(p => p.Bills)
                    .ThenInclude(p => p.Accounts)
                .Where(p => p.Status != 0)
                .ToList();

            return View(billDetail);
        }

        public ActionResult BaoHanh()
        {
            // List sản phẩm hoàn trả trong billDetail
            //var billDetail = _context.BillDetails
            //    .Include(p => p.PhoneDetaild)
            //        .ThenInclude(p => p.Phones)
            //    .Include(p => p.PhoneDetaild)
            //        .ThenInclude(p => p.Colors)
            //    .Include(p => p.PhoneDetaild)
            //        .ThenInclude(p => p.Rams)
            //    .Include(p => p.PhoneDetaild)
            //        .ThenInclude(p => p.Roms)
            //    .Include(p => p.Bills)
            //        .ThenInclude(p => p.Accounts)
            //    .Where (p => p.Status != 0)
            //    .ToList();

            //return View(billDetail);
            var warrantyCards = _context.WarrantyCards.ToList();
            return View(warrantyCards);
        }
        public ActionResult ChiTietBaoHanh(Guid id) 
        {
            // Hiển thị chi tiết điện thoại đổi trả + Thông tin khách hàng

                var warrantyCards = _context.WarrantyCards.Where(p => p.IdBillDetail == id).ToList();

            // Hiển thị thông tin khách hàng
            if (warrantyCards.Count() > 0)
            {
                // Lấy thông tin điện thoại trong billdetail
                ViewBag.BillDetails = _context.BillDetails
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
                                      .ToList();

                ViewBag.WarrantyCards = warrantyCards;

                var warrantyCard = warrantyCards.FirstOrDefault();
                var bill = _context.BillDetails.Include(p => p.Bills).FirstOrDefault(p => p.Id == warrantyCard.IdBillDetail);

                return View(bill);
            }

            return null;
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
            var bh = _context.WarrantyCards.SingleOrDefault(a => a.IdPhoneDetail == IdPhoneDetail && a.Imei == phoneImei);
            if (null != bh)
            {
                bh.Status = 1;
                bh.AppointmentDate = bh.CreatedDate.AddMonths(4);
                _context.SaveChanges();

                // Gửi mail thông báo cho khách hàng về thời gian bảo hành/ địa chỉ email gửi trả hàng
                // Tìm ra email theo thông tin khách hàng
                var idBill = _context.BillDetails.FirstOrDefault(p => p.Id == bh.IdBillDetail);

                var billDetails = _context.Bill
                                 .Include(p => p.Accounts)
                                 .FirstOrDefault(p => p.Id == idBill.IdBill);

                SendEmail(billDetails.Accounts.Email, bh.AppointmentDate);
            }

            return RedirectToAction("ChiTietBaoHanh", new { id = bh.IdBillDetail });
        }

        public ActionResult HuyBaoHanh(Guid IdPhoneDetail, string phoneImei)
        {
            // Cập nhật trạng thái trong bảng BillDetail status = 2 (2: Chấp nhận trả hàng)
            var bh = _context.WarrantyCards.SingleOrDefault(a => a.IdPhoneDetail == IdPhoneDetail && a.Imei == phoneImei);
            if (null != bh)
            {
                bh.Status = null;
                _context.SaveChanges();
            }

            return RedirectToAction("BaoHanh");
        }



        // Xác nhận bảo hành thành công (Máy đã được trả về cho khách hàng)
        public ActionResult BaoHanhThanhCong(Guid IdPhoneDetail, string phoneImei)
        {
            // Cập nhật trạng thái trong bảng BillDetail status = 0 (0: Trạng thái ban đầu) (1 máy có nhiều lần bảo hành)
            var bh = _context.WarrantyCards.SingleOrDefault(a => a.IdPhoneDetail == IdPhoneDetail && a.Imei == phoneImei);
            if (null != bh)
            {
                bh.Status = 2;
                bh.AppointmentDate = DateTime.Now;
                _context.SaveChanges();
            }

            return RedirectToAction("BaoHanh");
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
                mailMessage.IsBodyHtml = true;
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
                <p>Hãy gửi tới địa chỉ sau đây: 123ACCC<p>
                <p>Thời gian bảo hành sẽ tính đến ngày: {dNgayGuiBaohanh}, kể từ ngày bạn gửi yêu cầu bảo hành.<p>
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
