using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PRO219_WebsiteBanDienThoai_FPhone.ViewModel;
using PRO219_WebsiteBanDienThoai_FPhone.ViewModel.NewFolder;
using PRO219_WebsiteBanDienThoai_FPhone.ViewModel.Provinces;

namespace PRO219_WebsiteBanDienThoai_FPhone.Controllers
{
    public class CheckoutController : Controller
    {
	    private HttpClient _client;
     
	    public CheckoutController(HttpClient client)
	    {
		    _client = client;
            _client.DefaultRequestHeaders.Add("token", "a799ced2-febc-11ed-a967-deea53ba3605");
        }
	    public async Task<IActionResult> Index()
        {
            CheckOutViewModel model = new CheckOutViewModel();
            var data = await (await _client.GetAsync("https://online-gateway.ghn.vn/shiip/public-api/master-data/province")).Content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(data))
            {
                var Province = JsonConvert.DeserializeObject<ApiResponse<Province>>(data);
                model.Provinces = Province.Data;
            }
            return View(model);
        }

        public async Task<JsonResult> GetProvince()
        {
            _client.DefaultRequestHeaders.Add("token", "a799ced2-febc-11ed-a967-deea53ba3605");
	       var data = await _client.GetAsync("https://online-gateway.ghn.vn/shiip/public-api/master-data/province");
	       var name =  data.Content.ReadAsStringAsync();
           return Json(name);
        }
    }
}
