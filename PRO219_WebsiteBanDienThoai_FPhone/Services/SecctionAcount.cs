using AppData.Models;
using Newtonsoft.Json;

namespace PRO219_WebsiteBanDienThoai_FPhone.Services
{
    public class SecctionAcount
    {
        public static void SetobjTojson(ISession session, object value, string key)
        {
            //Convert sang json

            var jsonString = JsonConvert.SerializeObject(value);
            session.SetString(key, jsonString);
        }
        public static List<Account> GetObjFromSession(ISession session, string key)
        {
            var data = session.GetString(key); // doc du lieu tu ss
            if (data != null)
            {
                var listobj = JsonConvert.DeserializeObject<List<Account>>(data);
                return listobj;
            }
            else
            {
                return new List<Account>();
            }
        }

        public static bool CheckProductInTK(Guid Id, List<Account> cartpd)
        {
            return cartpd.Any(p => p.Id == Id);
        }
    }
}
//var userId = HttpContext.Session.GetString("UserId");
//Guid accountId = Guid.Parse(acout);
//var datajson = await _client.GetStringAsync($"api/Cart/getById/{accountId}");
//var obj = JsonConvert.DeserializeObject<Cart>(datajson);
//if (obj == null)
//{
//    var cart = new Cart() { IdAccount = accountId };
//    var jsonData = JsonConvert.SerializeObject(cart);
//    HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

//    var response = await _client.PostAsync("api/Cart/add", content);
//    var cartItem = new CartDetails()
//    {

//        IdAccount = accountId,
//        IdPhoneDetaild = id,
//        Quantity = 1,
//        Status = 1
//    };
//    var contents = new StringContent(JsonConvert.SerializeObject(cartItem), Encoding.UTF8, "application/json");
//    var responses = await _client.PostAsync("api/CartDetail/add", content);

//    return RedirectToAction("Cart");
//}
//else
//{
//    var cartItem = new CartDetails()
//    {

//        IdAccount = accountId,
//        IdPhoneDetaild = id,
//        Quantity = 1,
//        Status = 1
//    };
//    var content = new StringContent(JsonConvert.SerializeObject(cartItem), Encoding.UTF8, "application/json");
//    var response = await _client.PostAsync("api/CartDetail/add", content);
//    return RedirectToAction("Cart");
//}


