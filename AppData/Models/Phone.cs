namespace AppData.Models
{
    public class Phone
    {
        public Guid Id { get; set; }
        
        public string PhoneName { get; set; }

        public string? Description { get; set; }

        public string? Image { get; set; }

        public Guid IdProductionCompany { get; set; }

        public Guid? IdWarranty { get; set; }
        public DateTime?  CreateDate { get; set; } = DateTime.Now;

        public virtual ProductionCompany? ProductionCompanies { get; set; }

        public virtual Warranty? Warranty { get; set; }
    }
}
