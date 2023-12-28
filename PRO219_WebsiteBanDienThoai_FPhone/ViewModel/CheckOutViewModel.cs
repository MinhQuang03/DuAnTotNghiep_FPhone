using System.ComponentModel.DataAnnotations;
using AppData.Models;
using AppData.ViewModels.Phones;
using PRO219_WebsiteBanDienThoai_FPhone.Models;
using PRO219_WebsiteBanDienThoai_FPhone.ViewModel.NewFolder;

namespace PRO219_WebsiteBanDienThoai_FPhone.ViewModel
{
    public class CheckOutViewModel
    {
        public Bill Bill { get; set; } 
        public List<Province> Provinces { get; set; } = new List<Province>();
        /// <summary>
        /// danh sách thông tin chi tiết điện thoại ( VW_PhoneDetail  )
        /// </summary>
        public List<VW_PhoneDetail> lstCartDetail { get; set; } = new List<VW_PhoneDetail>();
        [Display(Name = "Tỉnh/Thành phố")]
        public string Province { get; set; }
        [Display(Name = "Quận/Huyện")]
        public string District { get; set; }
        [Display(Name = "Xã/Phường")]
        public string Ward { get; set; }
        [Display(Name = "Địa chỉ chi tiết")]
        public string DetailedAddress { get; set; }
            //để tạm
        public string id { get; set; }
        [Display(Name = "Họ và tên")]
        public string Name { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;
        public DateTime PaymentDate { get; set; }
        [Display(Name = "Số điện thoại")]
        public string Phone { get; set; }
        public decimal TotalMoney { get; set; } 
        public decimal ToTalShip { get; set; }
        public string Address { get; set; }
        public int Status { get; set; }
        public Guid IdAccount { get; set; } 
    }
}
        