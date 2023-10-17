using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModels.Accounts
{
    public class AdSignUpViewModel
    {
        [Display(Name = "Họ và tên")]
        [Required]
        public string FullName { get; set; }
        [Display(Name = "Tên đăng nhập")]
        [Required]
        public string UserName { get; set; }
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }
        [Display(Name = "Mật khẩu")]
        [PasswordPropertyText, Required]
        public string Password { get; set; }
        [Display(Name ="Số điện thoại")]
        [Phone]
        public string PhoneNumber { get; set; }
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }
        [Display(Name = "Căn cước công dân")]
        public string CitizenId { get; set; }
        [Display(Name = "Trạng thái")]
        public int Status { get; set; }
        public string ImageUrl { get; set; }
        [Display(Name = "Chức vụ")]
        public string Role { get; set; }

    }
}
