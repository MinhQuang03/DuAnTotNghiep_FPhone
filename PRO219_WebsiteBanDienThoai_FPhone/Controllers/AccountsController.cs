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
using NuGet.Protocol.Plugins;
using System.Text;

namespace PRO219_WebsiteBanDienThoai_FPhone.Controllers;

public class AccountsController : Controller
{
    private ICartDetailRepository _cartDetailepository;
    private IcartRepository _cartRepository;
    private FPhoneDbContext _context;
    private readonly HttpClient _client;
    private IEmailService _emailService;
    private IAccountService _service;
    private IHttpContextAccessor _accessor;
    public AccountsController(HttpClient client, IEmailService emailService, IAccountService service, IHttpContextAccessor accessor)
    {
        _cartDetailepository = new CartDetailepository();
        _cartRepository = new CartRepository();
        _context = new FPhoneDbContext();
        _client = client;
        _emailService = emailService;
        _service = service;
        _accessor = accessor;
    }
    //Khi đã đăng nhập ấn nút có biểu tượng user sẽ hiện ra profile của người dùng
    public async Task<IActionResult> Profile()
    {
        // lấy ra id người dùng khi đã đăng nhập
        var id = User.Claims.FirstOrDefault(claim => claim.Type == "Id")?.Value;
     
        ViewBag.idacount = id;
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
                    Message = Utility.EmailCreateAccountTemplate(model.Name, model.Username)
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

    public IActionResult Login()
    {
        return View();
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

    public IActionResult ResetPassword()
    {
        return View();
    }
    [HttpPost]
    public IActionResult ResetPassword(ResetPasswordViewModel model)
    {
       model.Data = _service.GetUserByEmail(model.Email);
        if (model.Data == null)
        {
            ModelState.AddModelError("Email", "Không có tài khoản nào được đăng ký với địa chỉ email này");
            return View(model);
        }
        else
        {
            return RedirectToAction("CheckUser", new { model.Email });
        }

    }

    public IActionResult ChangePassword()
    {   
        ChangePasswordViewmodel model = new ChangePasswordViewmodel();
        var idUser = User.Claims.FirstOrDefault(claim => claim.Type == "Id")?.Value;
       
        if (idUser != null) model.Data = _service.GetUserById(Guid.Parse(idUser));
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewmodel model)
    {
        Security security = new Security();
        var captchaSS = GetSessionValue("_captcha_value"); //lấy captcha từ session
        model.Data = _service.GetUserById(model.Data.Id);

        //validate mật khẩu cũ
        if (model.Data.Password != security.Encrypt("B3C1035D5744220E", model.OldPassword))
        {
            ModelState.AddModelError("OldPassword", "Mật khẩu không trùng khớp");
            return View(model);
        }
        //validate xác nhận mật khẩu
        if (model.CfPassword != model.NewPassword)
        {
            ModelState.AddModelError("CfPassword", "Mật khẩu không trùng khớp");
            return View(model);
        }

        if (captchaSS != null && captchaSS == model.Captcha) 
        {
            model.Data.Password = security.Encrypt("B3C1035D5744220E", model.NewPassword); // mã hoá mật khẩu để lưu vào database
            _service.UpdateUser(model.Data.Id, model.Data, out DataError error);
            if (error.Success)
            {
                //đăng xuất khi đổi mật khẩu thành công
                var authenticationProperties = new AuthenticationProperties
                {
                    ExpiresUtc = DateTimeOffset.UtcNow.AddSeconds(20) // Thiết lập thời gian hết hạn sau khi đăng xuất
                };
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme, authenticationProperties);
                _accessor.HttpContext.User = null; // xoá thông tin user
                //Gửi email

                ObjectEmailInput emailInput = new ObjectEmailInput()
                {
                    FullName = model.Data.Name,
                    SendTo = model.Data.Email,
                    Subject = "Thông báo đổi mật khẩu",
                    Message = Utility.EmailChangePasswordTemplate(model.Data.Name)
                };
                await _emailService.SendEmail(emailInput); // gửi email
                return View("~/Views/Accounts/Success.cshtml", "Đổi mật khẩu thành công. Vui lòng đăng nhập lại");
            }
            else
            {
                return View(model);
            }
        }
        else
        {
            ModelState.AddModelError("Captcha", "Sai mã xác nhận");
            return View(model);
        }
    }
    public string GetSessionValue(string key)
    {
        if (_accessor != null && _accessor.HttpContext.Session != null)
        {
        
            string content = string.Empty;
            var bytes = default(byte[]);
            _accessor.HttpContext.Session.TryGetValue(key, out bytes);
            if (bytes != null && bytes.Count() > 0) content = Encoding.UTF8.GetString(bytes);
            return content;
        }
        else return null;
    }

    public IActionResult CheckUser(string email)
    {
        ResetPasswordViewModel model = new ResetPasswordViewModel();
        model.Email = email;
        model.Data = _service.GetUserByEmail(email);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> CheckUser(ResetPasswordViewModel model)
    {
        Security security = new Security();
        model.Data = _service.GetUserById(model.Data.Id);
        string randomPass = Utility.RandomString(8);
        string passwordHash = security.Encrypt("B3C1035D5744220E", randomPass);
        model.Data.Password = passwordHash;
        _service.UpdateUser(model.Data.Id, model.Data, out DataError error);
        if (error.Success)
        {
            ObjectEmailInput emailInput = new ObjectEmailInput()
            {
                FullName = model.Data.Name,
                SendTo = model.Data.Email,
                Subject = "Thông báo cấp lại mật khẩu",
                Message = Utility.EmailResetPasswordTemplate(model.Data.Name,randomPass)
            };
            await _emailService.SendEmail(emailInput); // gửi email
            return RedirectToAction("EmailSuccess");
        }

        return View(model);
    }

    public IActionResult EmailSuccess()
    {
        return View();
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
            if (product != null && product is List<CartDetailModel>)
            {
                TempData["count"] = (product as List<CartDetailModel>).Count;
            }
            else
            {
                // Xử lý khi đối tượng không tồn tại trong Session hoặc không phải là List<CartDetailModel>
                TempData["count"] = 0; // Hoặc giá trị mặc định tùy thuộc vào yêu cầu của bạn
            }
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
                var car = _context.CartDetails.Where(a =>a.IdAccount == (Guid.Parse(userId))).Count();
                if (car > 4)
                {
                    TempData["SuccessMessage"] = "Bạn chỉ được mua tối đa 5 sản phẩm !";
                    return RedirectToAction("ShowCart");
                }
                else
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
               
            }
            else

            {
                var car = _context.CartDetails.Where(a => a.IdAccount == (Guid.Parse(userId))).Count();
                if (car > 5)
                {
                    TempData["SuccessMessage"] = "Bạn chỉ được mua tối đa 5 sản phẩm !";
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
        int currentBillNumber;
        lock (_context.Bill)
        {
            currentBillNumber = _context.Bill.Count() + 1;
        }
        var billCode = "HD" + currentBillNumber.ToString("D5");
        Bill bill = new Bill();
        bill.Id = Guid.NewGuid();
        bill.Address = $"{order.Address},{order.Province},{order.District},{order.Ward}";
        bill.Name = order.Name;
        bill.BillCode = billCode;
        bill.Status = 0; // Chờ xác nhận 
        bill.TotalMoney = order.TotalMoney;
        bill.CreatedTime = DateTime.Now;
        bill.PaymentDate = DateTime.Now;
        bill.IdAccount = Guid.Parse(userId);
        bill.Phone = order.Phone;
        bill.StatusPayment = 0; // Chưa thanh toán 
        bill.deliveryPaymentMethod = "COD";

        TempData["Totalmeny"] = bill.TotalMoney.ToString();
        TempData["Totalship"] = order.ToTalShip.ToString();
        TempData["name"] = bill.Name;
        TempData["code"] = bill.BillCode;
        TempData["address"] = bill.Address;
        TempData["Phone"] = bill.Phone;
        TempData["StatusPayment"] = bill.StatusPayment.ToString(); // Chuyển sang string
        TempData["deliveryPaymentMethod"] = bill.deliveryPaymentMethod;
        _context.Bill.Add(bill);
        _context.SaveChanges();

        Guid idhd = bill.Id;

        var product = _context.CartDetails.Where(a => a.IdAccount == Guid.Parse(userId)).ToList();
        List<BillDetails> Listbill = new List<BillDetails>();


        foreach (var item in product)
        {
                // Thêm sản phẩm điện thoại vào bill detail
                BillDetails billDetail = new BillDetails();
                billDetail.IdBill = idhd;
                billDetail.Id = Guid.NewGuid();
                billDetail.IdPhoneDetail = item.IdPhoneDetaild;
                billDetail.Price = _context.PhoneDetailds.Find(item.IdPhoneDetaild).Price;
                billDetail.Number = 1;
                billDetail.Status = 0;
                
                Listbill.Add(billDetail);
            
        }  
        _context.BillDetails.AddRange(Listbill);
        await _context.SaveChangesAsync();

        return Json(new { success = true, data = "/Accounts/paymets" });

    }
    [HttpPost]
    public async Task<IActionResult> ORDERTT(CheckOutViewModel order)
    {

        var userId = User.Claims.FirstOrDefault(claim => claim.Type == "Id")?.Value;
        if (userId == null)
        {
            return BadRequest("User Id is not available.");
        }
        int currentBillNumber;
        lock (_context.Bill)
        {
            currentBillNumber = _context.Bill.Count() + 1;
        }
        var billCode = "HD" + currentBillNumber.ToString("D5");
        Bill bill = new Bill();
        bill.Id = Guid.NewGuid();
        bill.Address = "Nhận trực tiếp tại cửa hàng ";
        bill.Name = order.Name;
        bill.BillCode = billCode;
        bill.Status = 0; // Chờ xác nhận 
        bill.TotalMoney = order.TotalMoney;
        bill.CreatedTime = DateTime.Now;
        bill.PaymentDate = DateTime.Now;
        bill.IdAccount = Guid.Parse(userId);
        bill.Phone = order.Phone;
        bill.StatusPayment = 0; // Chưa thanh toán 
        bill.deliveryPaymentMethod = "TT";

        TempData["Totalmeny"] = bill.TotalMoney.ToString();
        TempData["Totalship"] = order.ToTalShip.ToString();
        TempData["name"] = bill.Name;
        TempData["code"] = bill.BillCode;
        TempData["address"] = bill.Address;
        TempData["Phone"] = bill.Phone;
        TempData["StatusPayment"] = bill.StatusPayment.ToString(); // Chuyển sang string
        TempData["deliveryPaymentMethod"] = bill.deliveryPaymentMethod;
        _context.Bill.Add(bill);
        _context.SaveChanges();

        Guid idhd = bill.Id;

        var product = _context.CartDetails.Where(a => a.IdAccount == Guid.Parse(userId)).ToList();
        List<BillDetails> Listbill = new List<BillDetails>();


        foreach (var item in product)
        {
            // Thêm sản phẩm điện thoại vào bill detail
            BillDetails billDetail = new BillDetails();
            billDetail.IdBill = idhd;
            billDetail.Id = Guid.NewGuid();
            billDetail.IdPhoneDetail = item.IdPhoneDetaild;
            billDetail.Price = _context.PhoneDetailds.Find(item.IdPhoneDetaild).Price;
            billDetail.Number = 1;
            billDetail.Status = 0;

            Listbill.Add(billDetail);

        }
        _context.BillDetails.AddRange(Listbill);
        await _context.SaveChangesAsync();

        return Json(new { success = true, data = "/Accounts/paymets" });

    }
    public async Task<IActionResult> PurchaseHistory(Guid idAccount)
    {
        
        var accBill = _context.Bill.Where(p => p.IdAccount == idAccount).OrderByDescending(p => p.CreatedTime).ToList();
        return View(accBill);
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
        var billDetail = _context.BillDetails.Include(a=>a.Bills)
                                             .ThenInclude(a=>a.Accounts)
                                             .Include(a=>a.PhoneDetaild)
                                             .SingleOrDefault(a => a.IdPhoneDetail == IdPhoneDetail && a.Imei == phoneImei);

        if (null != billDetail)
        {
            //billDetail.Status = 4; // Yêu cầu bảo hành 
            //_context.SaveChanges();

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

            TempData["SuccessMessage"] = "Bạn đã gửi yêu cầu thành công!";
            _context.WarrantyCards.Add(warrantyCard);
            _context.SaveChanges();
        }



        return RedirectToAction("XemChiTiet", new { idBill = billDetail.IdBill });
    }

    public ActionResult paymets()
    {
        var name = TempData["name"] as string;
        var code = TempData["code"] as string;
        var address = TempData["address"] as string;
        var phone = TempData["Phone"] as string;
        var statuspaymnt = TempData["StatusPayment"] as string;
        var deliverypaymethod = TempData["deliveryPaymentMethod"] as string;
        var totalmeny = TempData["Totalmeny"] as string;
        ViewBag.Totalmeny = totalmeny;
        ViewBag.name = name;
        ViewBag.code = code;
        ViewBag.address = address;
        ViewBag.phone = phone;
        ViewBag.statuspaymnt = statuspaymnt;
        ViewBag.deliverypaymethod = deliverypaymethod;
        var userId = User.Claims.FirstOrDefault(claim => claim.Type == "Id")?.Value;
        var product = _context.CartDetails.Where(a => a.IdAccount == Guid.Parse(userId)).ToList();
        decimal sum = 0;
        List<Payment> payments = new List<Payment>();
        foreach (var iten in product)
        {
            var idsp = _context.PhoneDetailds.FirstOrDefault(p => p.Id == iten.IdPhoneDetaild).IdPhone;
            var gia = _context.PhoneDetailds.FirstOrDefault(p => p.Id == iten.IdPhoneDetaild).Price;
            string anhsp = _context.Phones.FirstOrDefault(p => p.Id == idsp).Image;
            var tensp = _context.Phones.FirstOrDefault(p => p.Id == idsp).PhoneName;
            var idcolor = _context.PhoneDetailds.FirstOrDefault(p => p.Id == iten.IdPhoneDetaild).IdColor;
            var idRam = _context.PhoneDetailds.FirstOrDefault(p => p.Id == iten.IdPhoneDetaild).IdRam;
            var color = _context.Colors.FirstOrDefault(p => p.Id == idcolor).Name;
            var Ram = _context.Ram.FirstOrDefault(p => p.Id == idRam).Name;
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

        decimal ships = decimal.Parse(totalmeny) - sum;
        ViewBag.ships = ships;
        ViewBag.sum = sum;
        ViewBag.cart = payments;
        foreach (var item in product)
        {
            var cart = _context.CartDetails.Find(item.Id);
            _context.CartDetails.Remove(cart);
        }
        _context.SaveChanges();
        return View();
    }
}