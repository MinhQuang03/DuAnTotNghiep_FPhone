using System.Net.Http.Headers;
using AppData.FPhoneDbContexts;
using AppData.IServices;
using AppData.Models;
using AppData.ViewModels.Accounts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Filters;

namespace PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Controllers;

[Area("Admin")]
[AuthenFilter]
public class SignUpController : Controller
{
    private readonly HttpClient _client;
    private readonly IHttpContextAccessor _contextAccessor;
    private IAccountService _service;

    public SignUpController(HttpClient client, IHttpContextAccessor contextAccessor,IAccountService service)
    {
        _client = client;
        _contextAccessor = contextAccessor;
        _service = service;
    }


    public async Task<IActionResult> SignUp()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SignUp(AdSignUpViewModel model)
    {
        var data = _service.GetUserByUserName(model.UserName);
        var admin = _service.GetByUserName(model.UserName);
        if (data==null && admin==null)
        {
            model.Status = 0;
            model.ImageUrl = string.Empty;
            var result = await _client.PostAsJsonAsync("/api/Accounts/SignUp/Admin", model);
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Accounts");
            }
        }
        else
        {
            ModelState.AddModelError("UserName", "Tên đăng nhập này đã tồn tại");
            return View(model);
        }
       

        
        return View(model);
    }
}