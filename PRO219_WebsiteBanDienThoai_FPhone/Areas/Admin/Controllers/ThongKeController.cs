using AppData.FPhoneDbContexts;
using AppData.IServices;
using AppData.Models;
using AppData.Repositories;
//using AppData.ViewModels.DanhGia;

using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Filters;
using System.Globalization;

namespace App_View.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AuthenFilter]
    public class ThongKeController : Controller
    {


        public IvOverViewServices _overview;
        public readonly HttpClient _httpClient;

        public ThongKeController(IvOverViewServices overview, HttpClient httpClient)
        {
            _overview = overview;
            _httpClient = httpClient;
        }

        public IActionResult OverView()
        {
            var model =  _overview.listOverViewGroup().FirstOrDefault();
            return View(model);
        }

    }
}
