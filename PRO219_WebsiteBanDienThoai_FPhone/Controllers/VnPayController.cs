using AppData.FPhoneDbContexts;
using AppData.Models;
using Microsoft.AspNetCore.Mvc;
using PRO219_WebsiteBanDienThoai_FPhone.Helpers;
using PRO219_WebsiteBanDienThoai_FPhone.Models;
using PRO219_WebsiteBanDienThoai_FPhone.ViewModel;
using System.Drawing.Drawing2D;

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
            pay.AddRequestData("vnp_Amount", request.TotalMoney.ToString()); //số tiền cần thanh toán, công thức: số tiền * 100 - ví dụ 10.000 (mười nghìn đồng) --> 1000000
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

            // var order = new Order(){
            //     // các thông tin khác của đơn hàng
            //     // ...
            // }
            // luu đơn hàng xuống db... với trang thái là chưa duyệt... nhớ kèm theo mã order để tí nữa lấy ra check
            //  pay.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString()); //mã hóa đơn
            /// code here 
            /// 

            //var billId = Guid.NewGuid();
            //var bill = new Bill()
            //{
            //    Id = billId,
            //    CreatedTime = DateTime.Now,
            //    PaymentDate = DateTime.Now,
            //    Name = request.InfoShip.FullName,
            //    Address = request.InfoShip.Address,
            //    Phone = request.InfoShip.Phone,
            //    Status = 0,
            //    StatusPayment = 1, // VNPAY
            //    IdAccount = null,
            //    // BillCode = pay.GetResponseData("vnp_TxnRef"),  /// Thêm db ok thì mở cái này
            //};
            //_dbContext.Bill.Add(bill);
            //var billDetails = new List<BillDetails>();
            //foreach (var phone in request.Phones)
            //{
            //    billDetails.Add(new BillDetails()
            //    {
            //        Id = Guid.NewGuid(),
            //        IdBill = billId,
            //        IdPhoneDetail = Guid.Parse(phone.PhoneDetailId),
            //        Number = 1,
            //        Price = phone.Price,
            //        NameImei = "Sao ko de null",
            //        IdDiscount = null,
            //        Status = 0
            //    });
            //}
            //_dbContext.BillDetails.AddRange(billDetails);
            //_dbContext.SaveChanges();


            var userId = User.Claims.FirstOrDefault(claim => claim.Type == "Id")?.Value;
            if (userId == null)
            {
                return BadRequest("User Id is not available.");
            }
            Bill bill = new Bill();
            bill.Id = new Guid();
            bill.Address = request.Address + "," + request.Province + "," + request.District + "," + request.Ward;
            bill.Name = request.Name;
            bill.Status = 0;
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
                BillDetails billDetail = new BillDetails();
                billDetail.IdBill = idhd;
                billDetail.Id = new Guid();
                billDetail.IdPhoneDetail = item.IdPhoneDetaild;
                billDetail.Price = _dbContext.PhoneDetailds.Find(item.IdPhoneDetaild).Price;
                billDetail.Status = 0;
                Listbill.Add(billDetail);
            }
            foreach (var item in product)
            {
                var cart = _dbContext.CartDetails.Find(item.Id);
                _dbContext.CartDetails.Remove(cart);

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
                        /// Thêm db ok thì mở cái này. để change status
                        var bill = _dbContext.Bill.FirstOrDefault(x => x.BillCode == orderId);
                        if (bill != null)
                        {
                            bill.Status = 1; // đã xác nhận toán
                            bill.StatusPayment = 1; // Đã thanh toán 
                            bill.PaymentDate = DateTime.Now;
                            _dbContext.SaveChanges();
                        }
                    }
                    else
                    {
                        //Thanh toán không thành công. Mã lỗi: vnp_ResponseCode
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
    }
}
