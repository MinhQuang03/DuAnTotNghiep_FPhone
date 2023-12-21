using AppData.Models;
using AppData.ViewModels.Options;
using AppData.ViewModels.Phones;

namespace PRO219_WebsiteBanDienThoai_FPhone.ViewModel
{
    public class AdPhoneViewModel
    {
        public List<Phone> ListPhone { get; set; } = new List<Phone>();
        public VW_Phone_Group SearchData { get; set; } = new VW_Phone_Group();
        public ListOptions ListOptions { get; set; } = new ListOptions();
        public List<VW_Phone_Group> ListVwPhoneGroup { get; set; } = new List<VW_Phone_Group>();
    }
}
