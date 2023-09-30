using System.Net.Http.Headers;
using AppData.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Filters;

namespace PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Controllers;

[Area("Admin")]
public class SignUpController : Controller
{
    private readonly HttpClient _client;
    private readonly IHttpContextAccessor _contextAccessor;

    public SignUpController(HttpClient client, IHttpContextAccessor contextAccessor)
    {
        _client = client;
        _contextAccessor = contextAccessor;
    }

    [AuthorizationFilter("Admin")]
    public async Task<IActionResult> Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult SignUpAdmin()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SignUp(SignUpModel model)
    {
        var token = _contextAccessor.HttpContext.Request.Cookies["token"];
        var content = await _client.GetStringAsync("/api/AccountStaff/get-all-staff");
        var account = JsonConvert.DeserializeObject<List<ApplicationUser>>(content).Count;
        model.Status = 1;
        model.ImageUrl = "";
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token);
        var result = await _client.PostAsJsonAsync("/api/AccountStaff/SignUp", model);
        if (result.IsSuccessStatusCode)
        {
            if (account == 0) // khi chưa có tài khoản nào ( signup với role admin )
                return RedirectToAction("Login", "LogIn");

            return RedirectToAction("Accounts", "Home"); // signup vs role staff
        }

        return RedirectToAction("Index");
    }
}