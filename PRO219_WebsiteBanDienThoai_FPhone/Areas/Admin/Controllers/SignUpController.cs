﻿using System.Net.Http.Headers;
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

    public SignUpController(HttpClient client, IHttpContextAccessor contextAccessor)
    {
        _client = client;
        _contextAccessor = contextAccessor;
    }


    public async Task<IActionResult> SignUp()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SignUp(AdSignUpViewModel model)    
    {
       
        model.Status = 0;
        model.ImageUrl = string.Empty;
        var result = await _client.PostAsJsonAsync("/api/Accounts/SignUp/Admin", model);
        if (result.IsSuccessStatusCode)
        {
            return RedirectToAction("Index", "Accounts"); 
        }

        return View();
    }
}