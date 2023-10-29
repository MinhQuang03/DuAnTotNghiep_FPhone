using AppData.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using PRO219_WebsiteBanDienThoai_FPhone.Models;
using System.Net.Http;

namespace PRO219_WebsiteBanDienThoai_FPhone.Controllers
{
    public class ListPhoneController : Controller
    {
        private readonly HttpClient _client;

        public ListPhoneController(HttpClient client)
        {
            _client = client;
            
        }
        public async Task<IActionResult> Index()
        {
            

            var datajson = await _client.GetStringAsync("api/PhoneDetaild/get");
            var ctsp = JsonConvert.DeserializeObject<List<PhoneDetaild>>(datajson);
            
            var lstspView = from a in ctsp
                            group a by new
                            {
                                a.Phones.PhoneName,
                                a.Phones.Id,
                                a.Phones.Image
                            }
                into b
                            select new ProductView
                            {
                                IdProduct = b.Key.Id,
                                ProductName = b.Key.PhoneName,
                                Price = b.Select(c => c.Price).Min().ToString("C0") + " - " +
                                        b.Select(c => c.Price).Max().ToString("C0"),
                                Image = b.Key.Image
                            };
            return View(lstspView);
        }
    }
}
