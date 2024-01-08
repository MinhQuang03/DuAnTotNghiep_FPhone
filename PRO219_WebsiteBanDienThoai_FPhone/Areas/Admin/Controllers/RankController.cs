using AppData.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Filters;
using System.Text;

namespace PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[AuthenFilter]
    public class RankController : Controller
    {
        public readonly HttpClient _httpClient;
        public RankController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            var datajson = await _httpClient.GetStringAsync("api/Rank/get");
            var obj = JsonConvert.DeserializeObject<List<Rank>>(datajson);
            return View(obj.OrderBy(c =>c.STT));
        }
      
      
        
    }
}
