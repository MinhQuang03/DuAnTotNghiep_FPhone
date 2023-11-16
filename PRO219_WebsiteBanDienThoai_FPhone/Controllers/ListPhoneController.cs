using AppData.FPhoneDbContexts;
using AppData.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PRO219_WebsiteBanDienThoai_FPhone.Services;
using PRO219_WebsiteBanDienThoai_FPhone.ViewModel;


namespace PRO219_WebsiteBanDienThoai_FPhone.Controllers
{
    public class ListPhoneController : Controller
    {
        private readonly HttpClient _client;
        private FPhoneDbContext _context;
        public ListPhoneController(HttpClient client)
        {
            _context = new FPhoneDbContext();
            _client = client;
            
        }
        public async Task<IActionResult> Index()
        {

            var sl = 0;
            var userId = User.Claims.FirstOrDefault(claim => claim.Type == "Id")?.Value;
            if (userId == null)
            {
                var product = SessionCartDetail.GetObjFromSession(HttpContext.Session, "Cart");
                ViewBag.sl = product.Count;
            }
            else
            {
                var Cart = _context.CartsDetails.Where(a => a.IdAccount == (Guid.Parse(userId))).ToList();
                ViewBag.sl = Cart.Count;
            }
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
