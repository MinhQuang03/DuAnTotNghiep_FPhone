using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AppData.ViewModels.Accounts;

public class ClAccountsViewModel
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Display(Name = "Tên đăng nhập")]
    [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
    public string Username { get; set; }

    [Display(Name = "Mật khẩu")]
    [Required(ErrorMessage = "Mật khẩu không được để trống")]
    [MinLength(8,ErrorMessage = "Mật khẩu tối thiểu 8 kí tự bao gồm kí tự viết thường, viết hoa, kí tự đặc biệt")]
    public string Password { get; set; }

    [Display(Name = "Xác nhận mật khẩu")]
    [Required(ErrorMessage = "Mật khẩu không được để trống")]
    public string CfPassword { get; set; }

    [Display(Name = "Họ và tên")]
    [Required(ErrorMessage = "Họ và tên không được để trống")]
    public string? Name { get; set; }

    [Display(Name = "Email")]
    [Required(ErrorMessage = "Email không được để trống")]
    [EmailAddress(ErrorMessage = "Email sai định dạng")]
    public string Email { get; set; }

    [Phone]
    [Display(Name = "Số điện thoại")]
    [Required(ErrorMessage = "Số điện thoại không được để trống")]
    public string PhoneNumber { get; set; }

    public string? ImageUrl { get; set; }

    public int Status { get; set; }

    public int? Points { get; set; }
}

