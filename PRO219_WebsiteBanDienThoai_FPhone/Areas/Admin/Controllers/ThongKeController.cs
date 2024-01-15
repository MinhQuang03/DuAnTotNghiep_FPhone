﻿using AppData.FPhoneDbContexts;
using AppData.IServices;
using AppData.Models;
using AppData.Repositories;
using AppData.ViewModels.ThongKe;

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
        public IBillGanDayServices _billGanDayServices;
        public readonly HttpClient _httpClient;

        public ThongKeController(IvOverViewServices overview, HttpClient httpClient , IBillGanDayServices billganday)
        {
            _overview = overview;
            _httpClient = httpClient;
            _billGanDayServices = billganday;
        }

        public IActionResult OverView()
        {
            vOverView model = new vOverView();
                 model =  _overview.listOverViewGroup().FirstOrDefault();
                 model.billGanDay = _billGanDayServices.listBillGanDayViewGroup();
            return View(model);
        }

    }
}
