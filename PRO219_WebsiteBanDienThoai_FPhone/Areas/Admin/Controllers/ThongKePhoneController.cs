using AppData.FPhoneDbContexts;
using AppData.IServices;
using AppData.Models;
using AppData.Repositories;
using AppData.ViewModels.ThongKe;

//using AppData.ViewModels.DanhGia;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Filters;
using System.Globalization;

namespace App_View.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AuthenFilter]
    public class ThongKePhoneController : Controller
    {


        IPhoneStatiticsServices _phoneStatitics;
        public readonly HttpClient _httpClient;

        public ThongKePhoneController(IPhoneStatiticsServices statitics, HttpClient httpClient)
        {
            _phoneStatitics = statitics;
            _httpClient = httpClient;
        
        }

        public IActionResult PhoneStatitics()
        {
            PhoneStatitics model = new PhoneStatitics();
                 model =  _phoneStatitics.listPhoneStaticsGroup().FirstOrDefault();
 
            return View(model);
        }
        //public JsonResult Chart()
        //{
        //    vOverView model = new vOverView();
        //    model = _overview.listOverViewGroup().FirstOrDefault();
        //    model.billGanDay = _billGanDayServices.listBillGanDayViewGroup();
        //    return Json(model);
        //}

   

    }
}
