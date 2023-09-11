namespace AppData.Models
{
    public class BillDetails
    {
        public Guid Id { get; set; }

        public Guid IdBill { get; set; }
        public Guid? IdDiscount { get; set; }

        public int Number { get; set; }

        public decimal Price { get; set; }

        public int Status { get; set; }

    

        public virtual Bill Bills { get; set; }

        public virtual Discount Discounts { get; set; }
       

    }
}
