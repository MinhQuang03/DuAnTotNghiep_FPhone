using AppData.FPhoneDbContexts;
using AppData.Models;
using Microsoft.AspNetCore.Mvc;
using PRO219_WebsiteBanDienThoai_FPhone.Helpers;
using PRO219_WebsiteBanDienThoai_FPhone.Models;
using PRO219_WebsiteBanDienThoai_FPhone.ViewModel;
using System.Drawing.Drawing2D;
using System.Net.Mail;
using System.Net;

namespace PRO219_WebsiteBanDienThoai_FPhone.Controllers
{
    public class VnPayController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly FPhoneDbContext _dbContext;
        VNPAY vnpay = new VNPAY();
        public VnPayController(IConfiguration configuration, FPhoneDbContext dbContext)
        {
            _configuration = configuration;
            var vnpayConfig = _configuration.GetSection("VNPAY");
            vnpay = new VNPAY()
            {
                Url = vnpayConfig["Url"],
                ReturnUrl = vnpayConfig["ReturnUrl"],
                TmnCode = vnpayConfig["TmnCode"],
                HashSecret = vnpayConfig["HashSecret"]
            };
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Payment(CheckOutViewModel request)
        {
            var pay = new PayLib();

            pay.AddRequestData("vnp_Version", "2.1.0"); //Phiên bản api mà merchant kết nối. Phiên bản hiện tại là 2.1.0
            pay.AddRequestData("vnp_Command", "pay"); //Mã API sử dụng, mã cho giao dịch thanh toán là 'pay'
            pay.AddRequestData("vnp_TmnCode", vnpay.TmnCode); //Mã website của merchant trên hệ thống của VNPAY (khi đăng ký tài khoản sẽ có trong mail VNPAY gửi về)
            pay.AddRequestData("vnp_Amount", ((int)(request.TotalMoney) * 100).ToString()); //số tiền cần thanh toán, công thức: số tiền * 100 - ví dụ 10.000 (mười nghìn đồng) --> 1000000
            pay.AddRequestData("vnp_BankCode", ""); //Mã Ngân hàng thanh toán (tham khảo: https://sandbox.vnpayment.vn/apis/danh-sach-ngan-hang/), có thể để trống, người dùng có thể chọn trên cổng thanh toán VNPAY
            pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss")); //ngày thanh toán theo định dạng yyyyMMddHHmmss
            pay.AddRequestData("vnp_CurrCode", "VND"); //Đơn vị tiền tệ sử dụng thanh toán. Hiện tại chỉ hỗ trợ VND
            pay.AddRequestData("vnp_IpAddr", GetIpAddress()); //Địa chỉ IP của khách hàng thực hiện giao dịch
            pay.AddRequestData("vnp_Locale", "vn"); //Ngôn ngữ giao diện hiển thị - Tiếng Việt (vn), Tiếng Anh (en)
            pay.AddRequestData("vnp_OrderInfo", "Thanh toan hoa don"); //Thông tin mô tả nội dung thanh toán
            pay.AddRequestData("vnp_OrderType", "other"); //topup: Nạp tiền điện thoại - billpayment: Thanh toán hóa đơn - fashion: Thời trang - other: Thanh toán trực tuyến
            pay.AddRequestData("vnp_ReturnUrl", vnpay.ReturnUrl); //URL thông báo kết quả giao dịch khi Khách hàng kết thúc thanh toán
            string billCode = DateTime.Now.Ticks.ToString();
            pay.AddRequestData("vnp_TxnRef", billCode); //mã hóa đơn

            string paymentUrl = pay.CreateRequestUrl(vnpay.Url, vnpay.HashSecret);




            var userId = User.Claims.FirstOrDefault(claim => claim.Type == "Id")?.Value;
            if (userId == null)
            {
                return BadRequest("User Id is not available.");
            }
            Bill bill = new Bill();
            bill.Id = new Guid();
            bill.Address = request.Address + "," + request.Province + "," + request.District + "," + request.Ward;
            bill.Name = request.Name;
            bill.Status = 0; // cho xac nhan
            bill.TotalMoney = request.TotalMoney;
            bill.CreatedTime = DateTime.Now;
            bill.PaymentDate = DateTime.Now;
            bill.IdAccount = Guid.Parse(userId);
            bill.Phone = request.Phone;
            bill.StatusPayment = 0; // Chưa thanh toán 
            bill.deliveryPaymentMethod = "VNPAY";
            bill.BillCode = billCode;
            _dbContext.Bill.Add(bill);
            _dbContext.SaveChanges();
            Guid idhd = bill.Id;
            var product = _dbContext.CartDetails.Where(a => a.IdAccount == Guid.Parse(userId)).ToList();

            List<BillDetails> Listbill = new List<BillDetails>();

            foreach (var item in product)
            {
                // Thêm sản phẩm điện thoại vào bill detail
                BillDetails billDetail = new BillDetails();
                billDetail.IdBill = idhd;
                billDetail.Id = Guid.NewGuid();
                billDetail.IdPhoneDetail = item.IdPhoneDetaild;
                billDetail.Price = _dbContext.PhoneDetailds.Find(item.IdPhoneDetaild).Price;
                billDetail.Number = 1;
                billDetail.Status = 0;
                Listbill.Add(billDetail);
            }

            _dbContext.BillDetails.AddRange(Listbill);
            _dbContext.SaveChanges();

            //return RedirectToAction("PaymentConfirm");
            return Json(new { success = true, data = paymentUrl });
        }

        public ActionResult PaymentConfirm()
        {
            if (HttpContext.Request.Query.Count > 0)
            {
                string hashSecret = vnpay.HashSecret;
                var vnpayData = HttpContext.Request.Query;
                PayLib pay = new PayLib();

                //lấy toàn bộ dữ liệu được trả về
                foreach (string s in vnpayData.Keys)
                {
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        pay.AddResponseData(s, vnpayData[s]);
                    }
                }

                string orderId = pay.GetResponseData("vnp_TxnRef"); //mã hóa đơn
                long vnpayTranId = Convert.ToInt64(pay.GetResponseData("vnp_TransactionNo")); //mã giao dịch tại hệ thống VNPAY
                string vnp_ResponseCode = pay.GetResponseData("vnp_ResponseCode"); //response code: 00 - thành công, khác 00 - xem thêm https://sandbox.vnpayment.vn/apis/docs/bang-ma-loi/
                string vnp_SecureHash = HttpContext.Request.Query["vnp_SecureHash"]; //hash của dữ liệu trả về

                bool checkSignature = pay.ValidateSignature(vnp_SecureHash, hashSecret); //check chữ ký đúng hay không?

                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00")
                    {
                        //Thanh toán thành công
                        ViewBag.Message = "Thanh toán thành công hóa đơn " + orderId + " | Mã giao dịch: " + vnpayTranId;

                        /// Xử lý update lại trang thái đơn hàng đã thanh toán thành công.. theo Mã bill
                        //var orderIdString = orderId.ToString();
                        var bill = _dbContext.Bill.FirstOrDefault(x => x.BillCode == orderId);
                        if (bill != null)
                        {

                            bill.StatusPayment = 1; // Đã thanh toán 
                            bill.PaymentDate = DateTime.Now;
                            _dbContext.SaveChanges();
                        }

                        var userId = User.Claims.FirstOrDefault(claim => claim.Type == "Id")?.Value;
                        var product = _dbContext.CartDetails.Where(a => a.IdAccount == Guid.Parse(userId)).ToList();

                        // Lưu thông tin để hiển thị 
                        ViewBag.name = bill.Name;
                        ViewBag.code = bill.BillCode;
                        ViewBag.address = bill.Address;
                        ViewBag.phone = bill.Phone;
                        ViewBag.deliverypaymethod = bill.deliveryPaymentMethod;
                        ViewBag.paymentStatus = bill.StatusPayment;
                        ViewBag.Totalmeny = bill.TotalMoney;
                        decimal sum = 0;
                        List<Payment> payments = new List<Payment>();
                        foreach (var iten in product)
                        {
                            var idsp = _dbContext.PhoneDetailds.FirstOrDefault(p => p.Id == iten.IdPhoneDetaild).IdPhone;
                            var gia = _dbContext.PhoneDetailds.FirstOrDefault(p => p.Id == iten.IdPhoneDetaild).Price;
                            string anhsp = _dbContext.Phones.FirstOrDefault(p => p.Id == idsp).Image;
                            var tensp = _dbContext.Phones.FirstOrDefault(p => p.Id == idsp).PhoneName;
                            var idcolor = _dbContext.PhoneDetailds.FirstOrDefault(p => p.Id == iten.IdPhoneDetaild).IdColor;
                            var idRam = _dbContext.PhoneDetailds.FirstOrDefault(p => p.Id == iten.IdPhoneDetaild).IdRam;
                            var color = _dbContext.Colors.FirstOrDefault(p => p.Id == idcolor).Name;
                            var Ram = _dbContext.Ram.FirstOrDefault(p => p.Id == idRam).Name;
                            sum += gia;
                            Payment list = new Payment();
                            list.name = tensp;
                            list.price = gia;
                            list.img = anhsp;
                            list.color = color;
                            list.ram = Ram;
                            list.quantity = 1;
                            payments.Add(list);

                        }
                        decimal ships = (bill.TotalMoney - sum).Value;
                        ViewBag.ships = ships;
                        ViewBag.sum = sum;
                        ViewBag.cart = payments;


                        foreach (var item in product)
                        {
                            var cart = _dbContext.CartDetails.Find(item.Id);
                            _dbContext.CartDetails.Remove(cart);

                        }
                        var acc = _dbContext.Accounts.FirstOrDefault(x => x.Id == bill.IdAccount);
                        SendEmailVnPay(acc.Email);

                        _dbContext.SaveChanges();
                    }
                    else
                    {
                        //Thanh toán không thành công. Mã lỗi: vnp_ResponseCode

                        var billToDelete = _dbContext.Bill.FirstOrDefault(x => x.BillCode == orderId);
                        if (billToDelete != null)
                        {
                            var billDetailsToDelete = _dbContext.BillDetails.Where(x => x.IdBill == billToDelete.Id).ToList();
                            // Remove Bill and associated BillDetails
                            _dbContext.BillDetails.RemoveRange(billDetailsToDelete);
                            _dbContext.Bill.Remove(billToDelete);
                            _dbContext.SaveChanges();
                        }


                        ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý hóa đơn " + orderId + " | Mã giao dịch: " + vnpayTranId + " | Mã lỗi: " + vnp_ResponseCode;



                    }
                }
                else
                {
                    ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý";
                }
            }

            return View();
        }
        private string GetIpAddress()
        {
            string ipAddress = "";
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                ipAddress = Request.Headers["X-Forwarded-For"];
            }
            else
            {
                ipAddress = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            }
            return ipAddress;
        }

        public async Task<IActionResult> SendEmailVnPay(string toEmail)
        {
            try
            {
                // Thông tin tài khoản email của bạn
                string fromEmail = "fphone.store.404@gmail.com";
                string password = "bdrczcwdttczwbsv";

                var acc = _dbContext.Accounts.FirstOrDefault(p => p.Email == toEmail);

                // Tạo đối tượng MailMessage
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(fromEmail);
                mailMessage.To.Add(toEmail);
                mailMessage.Subject = "Thông báo đặt hàng thành công";
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
                <p>Cảm ơn quý khách đã tin tưởng <strong> FPHONE STORE. </strong> của chúng tôi</p>
                <p>Đơn hàng của quý khách đã được đạt hàng thành công.</p>
                <p>Nhân viên của chúng tôi sẽ liên hệ với quý khách sớm nhất để xác nhận đơn hàng.</p>
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
