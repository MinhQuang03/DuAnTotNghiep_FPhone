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
using AppData.ViewModels;
using PRO219_WebsiteBanDienThoai_FPhone.ViewModel;
using Serilog;

namespace PRO219_WebsiteBanDienThoai_FPhone.Controllers;

public class AccountsController : Controller
{
    private ICartDetailRepository _cartDetailepository;
    private IcartRepository _cartRepository;
    private FPhoneDbContext _context;
    private readonly HttpClient _client;
    public AccountsController(HttpClient client)
    {
        _cartDetailepository = new CartDetailepository();
        _cartRepository = new CartRepository();
        _context = new FPhoneDbContext();
        _client = client;
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
            if (respo.IsSuccessStatusCode) return await Login(login);
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
                DataError error = new DataError();
                error.Success = true;
                error.Msg = "Đăng nhập thành công";
                return RedirectToAction("AddCart");
            }
        }
        else
        {
            DataError error = new DataError();
            error.Success = false;
            error.Msg = "Đăng nhập không thành công";
            ModelState.AddModelError("UserName", "Tài khoản hoặc mật khẩu sai");
          return  RedirectToAction("Index", "Home");
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
            product.Add(new CartDetailModel { phoneDetaild = _context.PhoneDetailds.Find(id),quantity = 1  });
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
                    BillDetails billDetail = new BillDetails();
                    billDetail.IdBill = idhd;
                    billDetail.Id = Guid.NewGuid();
                    billDetail.IdPhoneDetail = item.IdPhoneDetaild;
                    billDetail.Price = _context.PhoneDetailds.Find(item.IdPhoneDetaild).Price;
                    billDetail.Status = 0;
                    Listbill.Add(billDetail);
                }

                foreach (var item in product)
                {
                    var cart = _context.CartDetails.Find(item.Id);
                    _context.CartDetails.Remove(cart);
                }

                _context.BillDetails.AddRange(Listbill);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
           
        
      
    }

    public async Task<IActionResult> PurchaseHistory(Guid idAccount)
    {
        var accBill = _context.Bill.FirstOrDefault(p => p.IdAccount == idAccount);

        var phoneNames = (from bd in _context.BillDetails
                          join pdp in _context.PhoneDetailds on bd.IdPhoneDetail equals pdp.Id
                          join ph in _context.Phones on pdp.IdPhone equals ph.Id
                          where bd.IdBill == accBill.Id
                          select ph.PhoneName).FirstOrDefault();

        var ramName = (from bd in _context.BillDetails
                       join pdp in _context.PhoneDetailds on bd.IdPhoneDetail equals pdp.Id
                       join ph in _context.Ram on pdp.IdRam equals ph.Id
                       where bd.IdBill == accBill.Id
                       select ph.Name).FirstOrDefault();

        var colorName = (from bd in _context.BillDetails
                         join pdp in _context.PhoneDetailds on bd.IdPhoneDetail equals pdp.Id
                         join ph in _context.Colors on pdp.IdColor equals ph.Id
                         where bd.IdBill == accBill.Id
                         select ph.Name).FirstOrDefault();

        // Lấy danh sách các PhoneName và gán vào ViewBag
        ViewBag.PhoneNames = phoneNames + " " + ramName + " " + colorName;

        var lisst = _context.BillDetails.Where(m => m.IdBill == accBill.Id).ToList();

        return View(lisst);
    }
}