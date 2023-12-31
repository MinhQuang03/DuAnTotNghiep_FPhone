using System.ComponentModel.DataAnnotations;

namespace PRO219_WebsiteBanDienThoai_FPhone.ViewModel
{
    public class ContactViewModel
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        [Display(Name = "Họ và tên")]
        [Required(ErrorMessage = "Vui lòng nhập thông tin")]
        public string FullName { get; set; }
        [Display(Name = "Email")]
        [EmailAddress]
        [Required(ErrorMessage = "Vui lòng nhập thông tin")]
        public string Email { get; set; }
        [Display( Name = "SĐT")]
        [Required(ErrorMessage = "Vui lòng nhập thông tin")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Nội dung")]
        public string Content { get; set; }
        public int Status { get; set; }
        public string Type { get; set; } 
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }
        [Display(Name = "Chủ đề")]
        public string Topic { get; set; }
    }
}
