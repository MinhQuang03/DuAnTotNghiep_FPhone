namespace AppData.Models
{
    public class Payment
    {
        public Guid Id { get; set; }

        public string PaymentMethod { get; set; }

        public string? Account { get ; set; }

        public int? CardId { get; set; }

        public DateTime? ExpiredDate { get; set; }

        public int? CvvCode { get; set; }

        public string? QrCode { get; set; }
    }
}
