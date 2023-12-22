using AppData.Models;
using AppData.ViewModels.Accounts;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using PRO219_WebsiteBanDienThoai_FPhone.Services;
using PRO219_WebsiteBanDienThoai_FPhone.Models;
using System.Text;
using System.Net.Http;
using AppData.FPhoneDbContexts;
using AppData.Repositories;
using AppData.IRepositories;
using AppData.IServices;
using AppData.Utilities;
using AppData.ViewModels;
using PRO219_WebsiteBanDienThoai_FPhone.ViewModel;
using Serilog;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace PRO219_WebsiteBanDienThoai_FPhone.Controllers;

public class AccountsController : Controller
{
    private ICartDetailRepository _cartDetailepository;
    private IcartRepository _cartRepository;
    private FPhoneDbContext _context;
    private readonly HttpClient _client;
    private IEmailService _emailService;
    public AccountsController(HttpClient client, IEmailService emailService)
    {
        _cartDetailepository = new CartDetailepository();
        _cartRepository = new CartRepository();
        _context = new FPhoneDbContext();
        _client = client;
        _emailService = emailService;
    }
    //Khi đã đăng nhập ấn nút có biểu tượng user sẽ hiện ra profile của người dùng
    public async Task<IActionResult> Profile()
    {
        // lấy ra id người dùng khi đã đăng nhập
        var id = User.Claims.FirstOrDefault(claim => claim.Type == "Id")?.Value;
        // lấy ra thông tin người dùng thông qua id
        var datajson = await _client.GetStringAsync($"api/Accounts/get-user/{id}");
        var user = JsonConvert.DeserializeObject<Account>(datajson);
        if (user != null)
        {
            var jsondata = await _client.GetStringAsync($"api/Address/get-address/{id}");
            var address = JsonConvert.DeserializeObject<Address>(jsondata);
            // gộp địa chỉ
            if (address != null)
                ViewBag.Address = address.HomeAddress + ", " + address.District + ", " + address.City + ", " +
                                  address.Country;
            return View(user);
        }

        return View();
    }

    public IActionResult CreateAccount()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateAccount(ClAccountsViewModel model)
    {
        // lấy ra tài khoản admin để kiểm tra username có trùng hay không
        var result = await _client.GetStringAsync("/api/Accounts/get-all-staff");
        var userName = JsonConvert.DeserializeObject<List<ApplicationUser>>(result);
        //Kiểm tra username nhập vào có tồn tại trong list tài khoản admin
        if (userName.Any(c => c.UserName == model.Username))
        {
            ModelState.AddModelError("Username", "Tên đăng nhập đã tồn tại");
            return View(model);
        }

        if (model.Password == model.CfPassword)
        {
            model.ImageUrl = string.Empty;
            model.Points = 0;
            model.Status = 0;
            var respo = await _client.PostAsJsonAsync("/api/Accounts/SignUp/Client", model);
            // gán lại giá trị cho LoginModel để đăng nhập
            var login = new LoginModel();
            login.UserName = model.Username;
            login.Password = model.Password;
            //khi tạo tài khoản thành công sẽ đăng nhập luôn
            if (respo.IsSuccessStatusCode)
            {
                ObjectEmailInput emailInput = new ObjectEmailInput()
                {
                    FullName = model.Name,
                    SendTo = model.Email,
                    Subject = "Thông báo tạo tài khoản",
                    UserName = model.Username,
                };
                await _emailService.SendEmail(emailInput); // gửi email
                return await Login(login);
            }
        }
        else
        {
            ModelState.AddModelError("CfPassword", "Mật khẩu không trùng khớp");
            return View(model);
        }

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginModel model)
    {
        var handler = new JwtSecurityTokenHandler();
        var result = await (await _client.PostAsJsonAsync("/api/Accounts/Login", model)).Content.ReadAsStringAsync();
        var respo = JsonConvert.DeserializeObject<LoginResponseVM>(result);
        if (respo != null && respo.Roles != null && respo.Token != null)
        {
            var options = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7) // Thời gian hết hạn của cookie
            };
            var token = respo.Token;

            var claimsPrincipal = handler.ReadJwtToken(token);
            var claims = claimsPrincipal.Claims;
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            // chuyển hướng đế trang admin
            if (respo.Roles.Contains("Admin") || respo.Roles.Contains("Staff")) return RedirectPermanent("/admin/accounts/index");

            //chuyển hướng đến trang chủ của web
            if (respo.Roles.Contains("User"))
            {
                DataError error = new DataError() { Success = true };
                error.Success = true;
                error.Msg = "Đăng nhập thành công";
                TempData["DataError"] = Utility.ConvertObjectToJson(error);
                return RedirectToAction("AddCart");
            }
        }
        else
        {
            DataError error = new DataError();
            error.Success = false;
            error.Msg = "Đăng nhập không thành công";

            ModelState.AddModelError("UserName", "Tài khoản hoặc mật khẩu sai");
            return RedirectToAction("Index", "Home");

            // TempData["DataError"] = Utility.ConvertObjectToJson(error);
            // return  RedirectToAction("Index", "Home");

        }

        return NoContent();
    }
    public IActionResult checkoutID()
    {
        var userId = User.Claims.FirstOrDefault(claim => claim.Type == "Id")?.Value;

        if (userId == null)
        {
            TempData["SuccessMessage"] = "Bạn Phải Đăng nhập trước!";
            return RedirectToAction("Cart");
        }

        return RedirectToAction("Cart");
    }

    public async Task<IActionResult> AddCart()
    {
        var userId = User.Claims.FirstOrDefault(claim => claim.Type == "Id")?.Value;
        var product = SessionCartDetail.GetObjFromSession(HttpContext.Session, "Cart");
        if (product != null)
        {
            var idcartss = _context.Carts.FirstOrDefault(a => a.IdAccount == (Guid.Parse(userId)));
            if (idcartss != null)
            {
                foreach (var item in product)
                {
                    CartDetails cartDetails = new CartDetails();
                    cartDetails.Id = new Guid();
                    cartDetails.IdPhoneDetaild = item.phoneDetaild.Id;
                    cartDetails.IdAccount = Guid.Parse(userId);
                    cartDetails.Status = 1;
                    _context.CartDetails.Add(cartDetails);
                    _context.SaveChanges();
                }
            }
            else

            {
                Cart cart = new Cart();
                cart.IdAccount = Guid.Parse(userId);
                _context.Carts.Add(cart);
                _context.SaveChanges();
                foreach (var item in product)
                {
                    CartDetails cartDetails = new CartDetails();
                    cartDetails.Id = new Guid();
                    cartDetails.IdPhoneDetaild = item.phoneDetaild.Id;
                    cartDetails.IdAccount = Guid.Parse(userId);
                    cartDetails.Status = 1;
                    _context.CartDetails.Add(cartDetails);
                    _context.SaveChanges();
                }
            }
        }
        HttpContext.Session.Remove("Cart");
        return RedirectToAction("ShowCart");


    }

    public async Task<IActionResult> Cart()
    {
        var userId = User.Claims.FirstOrDefault(claim => claim.Type == "Id")?.Value;
        if (userId == null)
        {
            var product = SessionCartDetail.GetObjFromSession(HttpContext.Session, "Cart");
            return View(product);
        }

        return RedirectToAction("ShowCart");
    }


    public async Task<IActionResult> ShowCart()
    {
        var userId = User.Claims.FirstOrDefault(claim => claim.Type == "Id")?.Value;
        var Cart = _context.CartDetails.Where(a => a.IdAccount == (Guid.Parse(userId))).ToList();
        ViewBag.sl = Cart.Count;
        return View(Cart);
    }
    public IActionResult DeleteCartAccount(Guid id)
    {
        var cart = _context.CartDetails.FirstOrDefault(a => a.Id == id);
        _context.CartDetails.Remove(cart);
        _context.SaveChanges();
        return RedirectToAction("ShowCart");
    }
    public async Task<IActionResult> AddToCard(Guid id)
    {

        var userId = User.Claims.FirstOrDefault(claim => claim.Type == "Id")?.Value;
        var product = SessionCartDetail.GetObjFromSession(HttpContext.Session, "Cart");
        if (userId == null)
        {
            product.Add(new CartDetailModel { phoneDetaild = _context.PhoneDetailds.Find(id), quantity = 1 });
            SessionCartDetail.SetobjTojson(HttpContext.Session, product, "Cart");

            return RedirectToAction("Cart");
        }
        else
        {

            var idcartss = _context.Carts.FirstOrDefault(a => a.IdAccount == (Guid.Parse(userId)));
            if (idcartss != null)
            {

                CartDetails cartDetails = new CartDetails();
                cartDetails.Id = new Guid();
                cartDetails.IdPhoneDetaild = id;
                cartDetails.IdAccount = Guid.Parse(userId);
                cartDetails.Status = 1;
                _context.CartDetails.Add(cartDetails);
                _context.SaveChanges();

                return RedirectToAction("ShowCart");
            }
            else

            {
                Cart cart = new Cart();
                cart.IdAccount = Guid.Parse(userId);
                _context.Carts.Add(cart);
                _context.SaveChanges();
                CartDetails cartDetails = new CartDetails();
                cartDetails.Id = new Guid();
                cartDetails.IdPhoneDetaild = id;
                cartDetails.IdAccount = Guid.Parse(userId);
                cartDetails.Status = 1;
                _context.CartDetails.Add(cartDetails);
                _context.SaveChanges();

                return RedirectToAction("ShowCart");
            }

        }

    }

    public IActionResult DeleteCart(Guid id)
    {
        var cart = SessionCartDetail.GetObjFromSession(HttpContext.Session, "Cart");

        // Tìm và xóa sản phẩm có ID tương ứng
        var productToRemove = cart.FirstOrDefault(p => p.phoneDetaild.Id == id);
        if (productToRemove != null)
        {
            cart.Remove(productToRemove);
            var jsonString = JsonConvert.SerializeObject(cart);
            HttpContext.Session.SetString("Cart", jsonString);
        }
        return RedirectToAction("Cart");
    }

    [HttpPost]
    public async Task<IActionResult> ORDER(CheckOutViewModel order)
    {

        var userId = User.Claims.FirstOrDefault(claim => claim.Type == "Id")?.Value;
        if (userId == null)
        {
            return BadRequest("User Id is not available.");
        }
        var currentBillNumber = _context.Bill.Count() + 1;
        var billCode = "HD" + currentBillNumber.ToString("D5");
        Bill bill = new Bill();
        bill.Id = Guid.NewGuid();
        bill.Address = $"{order.Address},{order.Province},{order.District},{order.Ward}";
        bill.Name = order.Name;
        bill.BillCode = billCode;
        bill.Status = 2; // Chờ xác nhận 
        bill.TotalMoney = order.TotalMoney;
        bill.CreatedTime = DateTime.Now;
        bill.PaymentDate = DateTime.Now;
        bill.IdAccount = Guid.Parse(userId);
        bill.Phone = order.Phone;
        bill.StatusPayment = 0; // Chưa thanh toán 
        bill.deliveryPaymentMethod = "COD";

        _context.Bill.Add(bill);
        _context.SaveChanges();

        Guid idhd = bill.Id;

        var product = _context.CartDetails.Where(a => a.IdAccount == Guid.Parse(userId)).ToList();


        List<BillDetails> Listbill = new List<BillDetails>();


        foreach (var item in product)
        {
            // Tìm ra imeil đầu tiên thuộc PhoneDetail có status = 1 (1: chưa được bán)
            var emeiCheck = _context.Imei.First(a => a.IdPhoneDetaild == item.IdPhoneDetaild && a.Status == 1);
            // trường hợp tồn tại emeiCheck
            if (null != emeiCheck)
            {
                // Cập nhật lại status = 2 (Đã bán)
                emeiCheck.Status = 2;
                _context.SaveChanges();

                // Thêm sản phẩm điện thoại vào bill detail
                BillDetails billDetail = new BillDetails();
                billDetail.IdBill = idhd;
                billDetail.Id = Guid.NewGuid();
                billDetail.IdPhoneDetail = item.IdPhoneDetaild;
                billDetail.Price = _context.PhoneDetailds.Find(item.IdPhoneDetaild).Price;
                billDetail.Number = 1;
                billDetail.Status = 0;
                billDetail.Imei = emeiCheck.NameImei; // Đúng tra là id của bảng emei. Nhưng name emei cũng không thể trùng.
                Listbill.Add(billDetail);
            }
        }

        foreach (var item in product)
        {
            var cart = _context.CartDetails.Find(item.Id);
            _context.CartDetails.Remove(cart);
        }

        _context.BillDetails.AddRange(Listbill);
        await _context.SaveChangesAsync();

        return Json(new { success = true });

        //  return RedirectToAction("Index", "Home");


    }

    public async Task<IActionResult> PurchaseHistory(Guid idAccount)
    {
        //var accBill = _context.Bill.FirstOrDefault(p => p.IdAccount == idAccount); // sao lại lấy 1 bill đầu. phải lấy hết chứ


        //var phoneNames = (from bd in _context.BillDetails
        //                  join pdp in _context.PhoneDetailds on bd.IdPhoneDetail equals pdp.Id
        //                  join ph in _context.Phones on pdp.IdPhone equals ph.Id
        //                  where bd.IdBill == accBill.Id
        //                  select ph.PhoneName).FirstOrDefault();

        //var ramName = (from bd in _context.BillDetails
        //               join pdp in _context.PhoneDetailds on bd.IdPhoneDetail equals pdp.Id
        //               join ph in _context.Ram on pdp.IdRam equals ph.Id
        //               where bd.IdBill == accBill.Id
        //               select ph.Name).FirstOrDefault();

        //var colorName = (from bd in _context.BillDetails
        //                 join pdp in _context.PhoneDetailds on bd.IdPhoneDetail equals pdp.Id
        //                 join ph in _context.Colors on pdp.IdColor equals ph.Id
        //                 where bd.IdBill == accBill.Id
        //                 select ph.Name).FirstOrDefault();

        //// Lấy danh sách các PhoneName và gán vào ViewBag
        //ViewBag.PhoneNames = phoneNames + " " + ramName + " " + colorName;

        //var lisst = _context.BillDetails.Where(m => m.IdBill == accBill.Id).ToList();

        //return View(lisst);

        // Lấy ra danh sách đơn hàng mà khách hàng đã đặt 
        var accBill = _context.Bill.Where(p => p.IdAccount == idAccount).OrderByDescending(p => p.CreatedTime).ToList();
        return View(accBill);
    }

    public ActionResult XemChiTiet(Guid idBill)
    {
        // Tìm ra thông tin chi tiết hóa đơn tương ứng theo mã Bill

        // Tên sản phầm gồm: Tên điện thoại => Lấy từ bảng "Phones"
        //                   Màu sắc        => Lấy từ bảng "Color"
        //                   Số Ram         => Lấy từ bảng "Ram"
        //                   Mã imeil       => Lấy từ bảng "Imeil"
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

    public ActionResult ThongTinBaoHanh(Guid idBill)
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
                    .Where(p => p.IdBill == idBill).ToList();
        return View(phone);
    }

    // Hoàn trả sản phẩm
    public ActionResult YeuCauTrahang(Guid IdPhoneDetail, string phoneImei)
    {
        // Trả từng sản phẩm trong giỏ hàng.
        // Trường hợp trong giỏ hàng trả hết => Bill có status = X (Đơn hàng có trạng thái hoàn trả)
        // Cập nhật lại số lượng trong kho(Emei)

        // Tìm sản phẩm muốn trả trong bảng BillDetails
        var billDetail = _context.BillDetails.SingleOrDefault(a => a.IdPhoneDetail == IdPhoneDetail && a.Imei == phoneImei);
        if (null != billDetail)
        {
            // Cập nhật trạng thái trong bảng BillDetail status = 1 (1: Yêu cầu hủy)
            billDetail.Status = 1;
            _context.SaveChanges();
        }

        return RedirectToAction("XemChiTiet", new { idBill = billDetail.IdBill });
    }

    // Yêu cầu bảo hành
    [HttpPost]
    public ActionResult YeuCauBaoHanh(Guid IdPhoneDetail, string phoneImei, string note)
    {
        // bảo hành từng sản phẩm trong giỏ hàng(BillDetail).

        // Cập nhật trạng thái trong bảng BillDetail status = 4 (4: Yêu cầu bảo hành)
        var billDetail = _context.BillDetails.Include(a=>a.Bills)
                                             .ThenInclude(a=>a.Accounts)
                                             .Include(a=>a.PhoneDetaild)
                                             .SingleOrDefault(a => a.IdPhoneDetail == IdPhoneDetail && a.Imei == phoneImei);

        if (null != billDetail)
        {
            billDetail.Status = 4; // Yêu cầu bảo hành 
            _context.SaveChanges();

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
            warrantyCard.Status = 1; // 1: Mới tạo
            // Bổ sung thêm các thông tin khác nếu cần

            _context.WarrantyCards.Add(warrantyCard);
            _context.SaveChanges();
        }



        return RedirectToAction("XemChiTiet", new { idBill = billDetail.IdBill });
    }
}