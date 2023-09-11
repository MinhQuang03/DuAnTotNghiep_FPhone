namespace AppData.Models
{
    public class Phone
    {
        public Guid Id { get; set; }
        
        public string PhoneName { get; set; }

        public string? Description { get; set; }

        public string? Image { get; set; }

        public Guid IdProductionCompany { get; set; }

        public virtual ProductionCompany? ProductionCompanies { get; set; }
    }
}
