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
    }   
    
}
