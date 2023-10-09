using AppData.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace PRO219_WebsiteBanDienThoai_FPhone.Controllers
{
    public class AccountsController : Controller
    {
        private readonly HttpClient _client;

        public AccountsController(HttpClient client)
        {
            _client = client;
        }

        public async Task<IActionResult> Profile()
        {
            string? id = User.Claims.FirstOrDefault(claim => claim.Type == "Id")?.Value;
            var datajson = await _client.GetStringAsync($"api/Accounts/get-user/{id}");
            var user = JsonConvert.DeserializeObject<Account>(datajson);
            if (user!=null)
            {
                var jsondata = await _client.GetStringAsync($"api/Address/get-address/{id}");
                var address = JsonConvert.DeserializeObject<Address>(jsondata);
                if (address != null) ViewBag.Address = address.HomeAddress + ", " + address.District + ", " + address.City + ", " + address.Country;
                return View(user);
            }
            return View();
        }

        public IActionResult CreateAccount()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateAccount(Account model)
        {
            return View();
        }
    }
}
