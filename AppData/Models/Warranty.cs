namespace AppData.Models
{
    public class Warranty
    {
        public Guid Id { get; set; }

        public DateTime? TimeWarranty { get; set; }

        public string? Description { get; set; }

       public int? Status { get; set; }
    }
}
