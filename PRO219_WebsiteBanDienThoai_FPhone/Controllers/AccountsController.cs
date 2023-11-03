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

namespace PRO219_WebsiteBanDienThoai_FPhone.Controllers;

public class AccountsController : Controller
{
    private FPhoneDbContext _context;
    private readonly HttpClient _client;
    public AccountsController(HttpClient client)
    {
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
                return RedirectToAction("Profile");
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

    public async Task<IActionResult> Cart()
    {
      
        var product = SessionCartDetail.GetObjFromSession(HttpContext.Session, "Cart");
  
        return View(product);
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

        return RedirectToAction("Cart");


    }
}