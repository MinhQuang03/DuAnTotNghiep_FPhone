using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModels.Accounts
{
    public class InputVM
    {
        [Required(ErrorMessage = "Không thể để trống tên đăng nhập.")]
        [StringLength(50, ErrorMessage = "Username nằm trong khoảng từ 5-50 ký tự", MinimumLength = 5)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Không thể để trống mật khẩu.")]
        [StringLength(100, ErrorMessage = "Mật khẩu phải có ít nhất 5 ký tự.", MinimumLength = 5)]
        public string Password { get; set; }
        [StringLength(100, ErrorMessage = "Tên phải có ít nhất 5 ký tự", MinimumLength = 5)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Không thể để trống địa chỉ email.")]
        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Không thể để trống số điện thoại.")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string PhoneNumber { get; set; }

    }
}
