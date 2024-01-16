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
using AppData.Utilities;

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
        [HttpGet("ThongKe/Chart")]
        public JsonResult Chart()
        {
            vOverView model = new vOverView();
            model = _overview.listOverViewGroup().FirstOrDefault();
            model.billGanDay = _billGanDayServices.listBillGanDayViewGroup();
            var bill = _billGanDayServices.BillThang();
            List<OverView2> over = new List<OverView2>()
            {
                new OverView2()
                {
                    BillStatus = "Chờ xác nhận",
                    CoutBill = bill.Count(c =>c.Status == FphoneConst.ChoXacNhan)
                },
                new OverView2()
                {
                    BillStatus = "Đã xác nhận",
                    CoutBill = bill.Count(c =>c.Status == FphoneConst.DaXacNhan)
                },
                new OverView2()
                {
                    BillStatus = "Đang giao",
                    CoutBill = bill.Count(c =>c.Status == FphoneConst.DangGiao)
                },
                new OverView2()
                {
                    BillStatus = "Hủy",
                    CoutBill = bill.Count(c =>c.Status == FphoneConst.Huy)
                },
                new OverView2()
                {
                    BillStatus = "Giao thất bại",
                    CoutBill = bill.Count(c =>c.Status == FphoneConst.GiaoThatBai)
                }
            };

            return Json(over);
        }
        [HttpGet]
        public IActionResult TiLeBill()
        {
            vOverView model = new vOverView();
            model = _overview.listOverViewGroup().FirstOrDefault();
            
            return View(model);
        }

    }
}
