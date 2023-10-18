using AppData.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PRO219_WebsiteBanDienThoai_FPhone.Models;
using PRO219_WebsiteBanDienThoai_FPhone.ViewModel;

namespace PRO219_WebsiteBanDienThoai_FPhone.Controllers
{
    public class PhoneDetailController : Controller
    {
        private HttpClient _client;

        public PhoneDetailController(HttpClient client)
        {
            _client = client;
        }
        public async Task<IActionResult> PhoneDetail(Guid Id)   
        {
            var datajson = await _client.GetStringAsync($"api/PhoneDetaild/get-detail/{Id}");
            var ctsp = JsonConvert.DeserializeObject<List<PhoneDetaild>>(datajson);
            var lstPhonedt = from a in ctsp 
                             group a by new
                             {
                                 a.Phones.PhoneName,
                                 a.Phones.Id,
                                 a.Phones.Image,
                                 a.Phones.Description,
                                 a.Phones.ProductionCompanies.Name,
                                 a.Images,
                                 a.Rams,
                                 a.Roms,
                              
                             } into b
                             select new ProductDetailView()
                             {
                                IdProductDetail = b.Select(c =>c.Id).ToList(),
                                Description = b.Key.Description,
                                IdProduct = b.Key.Id,
                                Brand = b.Key.Name,
                             
                                 ProductName = b.Key.PhoneName,
                                Color = b.Select(c =>c.Colors).ToList(),
                                Image = b.Key.Image,
                                Ram = b.Key.Rams,
                                Rom = b.Key.Roms
                             };
            var lst = lstPhonedt.ToList();
            return View(lst);
        }
    }
}
