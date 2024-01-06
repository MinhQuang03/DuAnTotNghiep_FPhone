using Newtonsoft.Json;
using PRO219_WebsiteBanDienThoai_FPhone.Models;

namespace PRO219_WebsiteBanDienThoai_FPhone.Services
{
    public class SessionService<T>
    {
        public static void SetobjTojson(ISession session, object value, string key)
        {
            //Convert sang json
            var jsonString = JsonConvert.SerializeObject(value);
            session.SetString(key, jsonString);
        }
        public static List<T> GetObjFromSession(ISession session, string key)
        {
            var data = session.GetString(key); // doc du lieu tu ss
            if (data != null)
            {
                var listobj = JsonConvert.DeserializeObject<List<T>>(data);
                return listobj;
            }
            else
            {
                return new List<T>();
            }
        }
    }
}
