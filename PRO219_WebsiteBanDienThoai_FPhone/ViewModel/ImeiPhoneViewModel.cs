using AppData.Models;

namespace PRO219_WebsiteBanDienThoai_FPhone.ViewModel
{
    public class ImeiPhoneViewModel
    {
        public Guid IdPhoneDetail { get; set; }

        public List<Imei> imeis { get; set; }

        public string PhoneDetailName { get; set; }

        public Imei AddImeiOfPhone { get; set; }

        public PhoneDetaild PhoneDetaild { get; set; } = new PhoneDetaild();
    }
}
