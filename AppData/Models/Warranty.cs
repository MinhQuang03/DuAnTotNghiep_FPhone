namespace AppData.Models
{
    public class Warranty
    {
        public Guid Id { get; set; }

        public int TimeWarranty { get; set; }

        public string? Description { get; set; }

       public int? Status { get; set; }
    }
}
