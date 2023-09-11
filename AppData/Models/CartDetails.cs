namespace AppData.Models
{
    public class CartDetails
    {
        public Guid Id { get; set; }    
        
        public Guid? IdAccount { get; set; }

        public Guid IdPhoneDetaild { get; set; }

        public int Quantity { get; set; }

        public int Status { get; set; }

        public virtual PhoneDetaild PhoneDetailds { get; set; }

        public virtual Cart Carts { get; set; }
    }
}
