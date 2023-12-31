
using System.Collections;
using AppData.Models;
using Microsoft.AspNetCore.Mvc;
using PRO219_WebsiteBanDienThoai_FPhone.ViewModel;

namespace PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdContactController : Controller   
    {
        public IActionResult Index()
        {
            IEnumerable<Contact> model = new List<Contact>();
            return View(model);
        }

        public IActionResult Detail()
        {
            return View();
        }
    }
}
