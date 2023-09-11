namespace AppData.Models
{
    public class Address
    {
        public Guid Id { get; set; }

        public Guid IdAccount { get; set; }

        public string? Country { get; set; }    

        public string? City { get; set; }

        public string? District { get; set; }

        public string? HomeAddress { get; set; }

        public int Status { get; set; }

        public virtual Account Accounts { get; set; }
    }
}
