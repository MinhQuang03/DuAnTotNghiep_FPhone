namespace AppData.Models
{
    public class Bill
    {
        public Guid Id { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime PaymentDate { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }    

        public int Status { get; set; }

        public Guid? IdAccount { get; set; }

        public virtual Account Accounts { get; set; }

    }
}
