using System.ComponentModel.DataAnnotations;

namespace AppData.Models
{
    public class Imei
    {
        public Guid Id { get; set; }

        public Guid? IdBillDetail { get; set; }


        [Required(ErrorMessage = "Tên Imei là trường bắt buộc.")]
        [StringLength(15, MinimumLength = 15, ErrorMessage = "Tên Imei phải có đúng 15 số.")]
        public string NameImei { get; set; }

        public int? Status { get; set; }

        public Guid? IdPhoneDetaild { get; set; }

        public virtual PhoneDetaild? PhoneDetaild { get; set; }
    }
}
