using AppData.Models;
using AppData.ViewModels.Options;

namespace PRO219_WebsiteBanDienThoai_FPhone.ViewModel
{
    public class AccountViewModel
    {
        public ListOptions Options { get; set; } = new ListOptions();
        public List<ApplicationUser> LstUser { get; set; }
        public ApplicationUser SearchData { get; set; } = new ApplicationUser();
    }
}
