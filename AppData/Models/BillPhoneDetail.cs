using System.ComponentModel.DataAnnotations;

namespace AppData.Models
{
    public class BillPhoneDetail
    {
        public Guid BillDetailId { get; set; }
        public Guid PhoneDetailId { get; set; }
        public virtual BillDetails? BillDetails { get; set; }
        public virtual PhoneDetaild? PhoneDetaild { get; set; }
    }
}
