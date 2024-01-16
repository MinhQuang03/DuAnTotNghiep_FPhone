using AppData.FPhoneDbContexts;
using AppData.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Filters;
using PRO219_WebsiteBanDienThoai_FPhone.ViewModel;

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
            SellDailyViewModel sellDailyViewModel = new SellDailyViewModel();
            var datajson = await _httpClient.GetStringAsync("api/SellDaillys/get");
            sellDailyViewModel.LstSellDailys = JsonConvert.DeserializeObject<List<SellDailys>>(datajson);
            return View(sellDailyViewModel);
        }
        [HttpPost]    
        public async Task<IActionResult> Index(SellDailyViewModel sellDailysViewmodel)
        {
            var datajson = await _httpClient.GetStringAsync("api/SellDaillys/get");
            var obj = JsonConvert.DeserializeObject<List<SellDailys>>(datajson);
            sellDailysViewmodel.LstSellDailys = obj.Where(c => (sellDailysViewmodel.Search.Month == null || c.CreateTime.Month == sellDailysViewmodel.Search.Month) && (sellDailysViewmodel.Search.Year == null || c.CreateTime.Year == sellDailysViewmodel.Search.Year)).ToList();
            return View(sellDailysViewmodel);
        }


        public async Task<IActionResult> Monthly()
        {
            SellMonthlyViewModel sellMonthlyViewModel = new SellMonthlyViewModel();
            var datajson = await _httpClient.GetStringAsync("api/SellMonthlys/get");
            sellMonthlyViewModel.lstsellMonthlys = JsonConvert.DeserializeObject<List<SellMonthlys>>(datajson);
            return View(sellMonthlyViewModel);
        }

    }
}
