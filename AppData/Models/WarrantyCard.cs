namespace AppData.Models
{
    public class WarrantyCard
    {
        public Guid Id { get; set; }

        public Guid IdBillDetail { get; set; }

        public DateTime CreatedDate { get; set; }

        public string? Description { get; set; }

        public DateTime? AppointmentDate { get; set; }

        public virtual BillDetails BillDetails { get; set; }
    }
}
