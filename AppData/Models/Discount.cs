namespace AppData.Models
{
    public class Discount
    {
        public Guid Id { get; set; }
        public Decimal ReducedAmount { get; set; }

        public DateTime TimeForm { get; set; }

        public DateTime? TimeTo { get; set; }

        public string? Note { get; set; }

        public int Status { get; set; }
    }
}
