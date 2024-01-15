using System.ComponentModel.DataAnnotations;
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

    public class AdPhoneInsertViewModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Display(Name = "Tên sản phẩm")]
       
        public string PhoneName { get; set; }
        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        public string? Image { get; set; }

        public Guid IdProductionCompany { get; set; }

        public Guid? IdWarranty { get; set; }
        public DateTime? CreateDate { get; set; } = DateTime.Now;

        public List<Warranty> ListWarranty { get; set; } = new List<Warranty>();
        public List<ProductionCompany> ListCompany { get; set; } = new List<ProductionCompany>();
    }
}
