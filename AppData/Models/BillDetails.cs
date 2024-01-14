namespace AppData.Models
{
    public partial class BillDetails
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid IdBill { get; set; }
        public Guid? IdDiscount { get; set; }

        public Guid IdPhoneDetail { get; set; }

        public int Number { get; set; }

        public decimal Price { get; set; }

        public int Status { get; set; } = 0;

        public string? Imei { get; set; }

        public DateTime? Update_at { get; set; }

        public string? Note { get; set; }

        public virtual Bill Bills { get; set; }

        public virtual Discount? Discounts { get; set; }

        public virtual PhoneDetaild PhoneDetaild { get; set; } 
       

    }
}
