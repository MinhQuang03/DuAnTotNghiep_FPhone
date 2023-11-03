using Newtonsoft.Json;
using PRO219_WebsiteBanDienThoai_FPhone.Models;
using PRO219_WebsiteBanDienThoai_FPhone.ViewModel;

namespace PRO219_WebsiteBanDienThoai_FPhone.Services
{
    public class SessionCartDetail
    {
        public static void SetobjTojson(ISession session, object value, string key)
        {
            //Convert sang json

            var jsonString = JsonConvert.SerializeObject(value);
            session.SetString(key, jsonString);
        }
        public static List<CartDetailModel> GetObjFromSession(ISession session, string key)
        {
            var data = session.GetString(key); // doc du lieu tu ss
            if (data != null)
            {
                var listobj = JsonConvert.DeserializeObject<List<CartDetailModel>>(data);
                return listobj;
            }
            else
            {
                return new List<CartDetailModel>();
            }
        }

        public static bool CheckProductIncart(Guid Id, List<CartDetailModel> cartpd)
        {
            return cartpd.Any(p => p.phoneDetaild.Id == Id);
        }
    }
}
