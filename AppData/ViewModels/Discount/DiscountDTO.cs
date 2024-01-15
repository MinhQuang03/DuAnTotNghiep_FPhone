using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModels.Discount
{
    public class DiscountDTO
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [StringLength(25, MinimumLength = 2, ErrorMessage = "Tên voucher phải từ 2 -25 kí tự")]

        public string? MaVoucher { get; set; }
        [Required(ErrorMessage = "Tên voucher là trường bắt buộc.")]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "Tên voucher phải nhập từ 2-25 kí tự")]
        [RegularExpression(@"^[a-zA-Z0-9\s]*$", ErrorMessage = "Tên voucher không được chứa ký tự đặc biệt.")]

        public string? NameVoucher { get; set; }
        [Required(ErrorMessage = "Điều kiện là trường bắt buộc")]
        [Range(0, int.MaxValue, ErrorMessage = " là số nguyên không âm")]

        public int? DieuKien { get; set; }
        [Required(ErrorMessage = "Loại khuyến mãi là bắt buộc")]
        public int? TypeVoucher { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Trường cần nhập tối đa từ 0 đến 999.999.999")]
        public int? Quantity { get; set; }

        public double? MucUuDai { get; set; }
        [Required(ErrorMessage = "Ngày bắt đầu là trường bắt buộc")]
        public DateTime DateStart { get; set; }
        [Required(ErrorMessage = "Ngày kết thúc là trường bắt buộc")]
        public DateTime DateEnd { get; set; }
        public string? StatusVoucher { get; set; }
        public int SoLuongVoucherDaIn { get; set; }
        public int SoLuongVoucherChuaIN { get; set; }
    }
    public class CustomMucUuDaiValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = (DiscountDTO)validationContext.ObjectInstance;

            if (model.TypeVoucher == 0)
            {
                if (value is double mucUuDai && mucUuDai <= 0)
                {
                    return new ValidationResult("Số tiền giảm phải lớn hơn 0.");
                }
            }
            else if (model.TypeVoucher == 1)
            {
                if (value is double mucUuDai && (mucUuDai <= 0 || mucUuDai > 100))
                {
                    return new ValidationResult("% giảm phải nằm trong khoảng từ 0 đến 100.");
                }
            }
            else if (model.TypeVoucher == 2)
            {
                // Kiểm tra mức ưu đãi theo điều kiện riêng cho LoaiHinhUuDai là 2
                // Điều kiện này tương tự với khi LoaiHinhUuDai là 0
                if (value is double mucUuDai && mucUuDai <= 0)
                {
                    return new ValidationResult("Số tiền giảm đãi phải lớn hơn 0.");
                }
            }

            return ValidationResult.Success;
        }
    }
    public class CustomNgayKetThucValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var ngayKetThuc = (DateTime)value;
            var model = (DiscountDTO)validationContext.ObjectInstance;

            if (ngayKetThuc <= model.DateStart)
            {
                return new ValidationResult("Ngày kết thúc phải lớn hơn hoặc bằng ngày bắt đầu.");
            }

            return ValidationResult.Success;
        }
    }
}
