using AppData.Models;
using AppData.ViewModels.Phones;

namespace PRO219_WebsiteBanDienThoai_FPhone.ViewModel
{
    public class VwProductDetailViewModel
    {
      public List<VW_PhoneDetail> Records = new List<VW_PhoneDetail>();
      public List<ListImage> lstImage = new List<ListImage>();
      public string? Image { get; set; }
      public List<VW_List_By_IdPhone> listImageByIdPhone = new List<VW_List_By_IdPhone>();
      public List<Ram> LstRam { get; set; } = new List<Ram>();
      public List<string> ListIDRam { get; set; } = new List<string>();
      public string IdPhone { get; set; }
      public string IdPhoneDetail { get; set; }
      public string? IdAccount { get; set; }
      public Phone Phone { get; set; } = new Phone();
      public List<Review> ListReview { get; set; } = new List<Review>();
    }   
    
}
