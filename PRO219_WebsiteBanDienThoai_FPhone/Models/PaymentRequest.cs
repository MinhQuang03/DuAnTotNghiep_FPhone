namespace PRO219_WebsiteBanDienThoai_FPhone.Models
{
    public class PaymentRequest
    {
        public string? Content { get; set; }
        public int Price { get; set; }

        public InfoShip? InfoShip { get; set; }
        public List<PhoneOrder>? Phones { get; set; }
    }
    public class PhoneOrder
    {
        public string? PhoneId { get; set; }
        public string? PhoneDetailId { get; set; }
        public int Price { get; set; }
    }
}
