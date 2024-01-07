﻿using System.Data.Entity.Infrastructure;
using AppData.FPhoneDbContexts;
using AppData.IRepositories;
using AppData.IServices;
using AppData.Models;
using AppData.Utilities;
using AppData.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

using PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Filters;
using PRO219_WebsiteBanDienThoai_FPhone.ViewModel;

namespace PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Controllers;

[Area("Admin")]
[AuthenFilter]

public class AccountsController : Controller
{
    private readonly HttpClient _client;
    private IAccountService _accountsService;
    private FPhoneDbContext _context;

    public AccountsController(HttpClient client, IAccountService accountsService)
    {
        _context = new FPhoneDbContext();
        _client = client;
        _accountsService = accountsService;
    }
    public IActionResult Index()
    {
        ViewBag.product = _context.WarrantyCards.Where(a=>a.Status == 0).Count();
        ViewBag.Neworder = _context.Bill.Where(a => a.Status == 2).Count();
        ViewBag.contact = _context.Bill.Where(a => a.Status == 3).Count();
        ViewBag.user = _context.Accounts.Count();
        return View();
    }

    public async Task<IActionResult> Account()
    {
        AccountViewModel model = new AccountViewModel();
        if (UserClaim.HasRole(User, "Admin"))
        {
            model.LstUser = _accountsService.GetAllAsync(model.SearchData, model.Options);
            return View(model);
        }

        return BadRequest();
    }
    [HttpPost]
    public async Task<IActionResult> Account(AccountViewModel model)
    {
        if (UserClaim.HasRole(User, "Admin"))
        {
            model.LstUser = _accountsService.GetAllAsync(model.SearchData, model.Options);
            return View(model);
        }
        return BadRequest();
    }
    [HttpGet]
    public async Task<IActionResult> EditAccount(string id)
    {
        if (id == null)
        {
            return RedirectToAction("Account");
        }
        var model = _accountsService.GetById(id);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> EditAccount(ApplicationUser model)
    {
        if (model.Images != null)
        {
            if (model.Images.FileName.Length > 0)
            {
                var fileName = Path.GetFileName(model.Images.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Images.CopyToAsync(stream);
                }
                model.ImageUrl = "/img/" + fileName;
            }
        }

        var result = _accountsService.Update(model.Id, model, out DataError error);
        if (error != null)
        {
            TempData["DataError"] = Utility.ConvertObjectToJson(error);
        }

        if (result != null)
        {
            return RedirectToAction("Account");
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult DetailAccount(string id)
    {
        ApplicationUser user = new ApplicationUser();
        if (id == null)
        {
            return RedirectToAction("Account");
        }
        user = _accountsService.GetById(id);
        return View(user);
    }


    public IActionResult Editkhachhang(Guid id)
    {
        Account Accounts = _context.Accounts.Find(id);
        Accounts.Status = 1;
        _context.SaveChanges();
        return RedirectToAction("KhachHang");
    }
    public IActionResult Editkhachhang1(Guid id)
    {
        Account Accounts = _context.Accounts.Find(id);
        Accounts.Status = 0;
        _context.SaveChanges();
        return RedirectToAction("KhachHang");
    }
    public async Task<RedirectResult> LogOut()
    {
        
        var authenticationProperties = new AuthenticationProperties
        {
            ExpiresUtc = DateTimeOffset.UtcNow.AddSeconds(20) // Thiết lập thời gian hết hạn sau khi đăng xuất
        };

        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme, authenticationProperties);
        Response.Cookies.Delete(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectPermanent("/home");
    }
    public async Task<IActionResult> KhachHang()
    {
        var acount = _context.Accounts.ToList();
        return View(acount);
    }
    public IActionResult AddSildeShow()
    {
        return View();
    }
}