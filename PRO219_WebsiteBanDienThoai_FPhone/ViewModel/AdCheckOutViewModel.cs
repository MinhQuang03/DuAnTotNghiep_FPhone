using AppData.Models;
using AppData.ViewModels.Phones;

namespace PRO219_WebsiteBanDienThoai_FPhone.ViewModel
{
    public class AdCheckOutViewModel
    {
        public List<VW_PhoneDetail> ListvVwPhoneDetails { get; set; } = new List<VW_PhoneDetail>();
        public List<ListImage> listImage { get; set; } = new List<ListImage>();
        public Bill Bill { get; set; } = new Bill();
        public List<BillDetails> ListBillDetail { get; set; } = new();
        public Account Account { get; set; } = new Account();
    }
}
