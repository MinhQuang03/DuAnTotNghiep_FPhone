namespace AppData.Models
{
    public class Refund
    {
        public Guid Id { get; set; }

        public string? Imei { get; set; }

        public string? PhoneName { get; set; }

        public decimal? Price { get; set; }

        public DateTime CreateDate { get; set; }

        public int? StatusDetail { get; set; }


    }
}
