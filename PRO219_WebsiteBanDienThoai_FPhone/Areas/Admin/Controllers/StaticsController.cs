using AppData.FPhoneDbContexts;
using AppData.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Filters;

namespace PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AuthenFilter]
    public class StaticsController : Controller
    {
        
        public FPhoneDbContext _dbContext;
        public readonly HttpClient _httpClient;


        public StaticsController(FPhoneDbContext dbContext, HttpClient httpClient)
        {
            _dbContext = dbContext;
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            var datajson = await _httpClient.GetStringAsync("api/SellDaillys/get");
            var obj = JsonConvert.DeserializeObject<List<SellDailys>>(datajson);
            return View(obj);
        }
    }
}
