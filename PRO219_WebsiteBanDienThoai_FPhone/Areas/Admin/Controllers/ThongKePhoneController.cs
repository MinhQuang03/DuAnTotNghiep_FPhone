using AppData.FPhoneDbContexts;
using AppData.IServices;
using AppData.Models;
using AppData.Repositories;
using AppData.Services;
using AppData.Utilities;
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


        [HttpGet("ThongKePhone/Chart1")]
        public JsonResult Chart()
        {
            PhoneStatitics model = new PhoneStatitics();
            model = _phoneStatitics.listPhoneStaticsGroup().FirstOrDefault();


            List<OverView2> over = new List<OverView2>()
            {
                new OverView2()
                {
                    BillStatus = "Buổi sáng",
                    CoutBill = model.BuoiSang.Value,
                },
                new OverView2()
                {
                   BillStatus = "Buổi Chiều",
                    CoutBill = model.BuoiChieu.Value,
                },
                new OverView2()
                {
                    BillStatus = "Buổi Tối",
                    CoutBill = model.BuoiToi.Value,
                },
               
            };

            return Json(over);
        }

        [HttpGet("ThongKePhone/Chart2")]
        public JsonResult Chart2()
        {
            PhoneStatitics model = new PhoneStatitics();
            model = _phoneStatitics.listPhoneStaticsGroup().FirstOrDefault();


            List<OverView2> over = new List<OverView2>()
            {
                new OverView2()
                {
                    BillStatus = "Phân khúc tầm thấp",
                    CoutBill = model.PhanKhucThap.Value,
                },
                new OverView2()
                {
                   BillStatus = "Phân khúc tầm trung",
                    CoutBill = model.PhanKhucTrungCap.Value,
                },
                new OverView2()
                {
                    BillStatus = "Phân khúc cao cấp",
                    CoutBill = model.PhanKhucCaoCap.Value,
                },

            };

            return Json(over);
        }



    }
}
