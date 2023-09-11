namespace AppData.Models
{
    public class Review
    {
        public Guid Id { get; set; }

        public DateTime DateTime { get; set; }

        public string? Content { get; set; }

        public Guid IdPhoneDetaild { get; set; }

        public Guid IdAccount { get; set; }

        public virtual PhoneDetaild PhoneDetailds { get; set; }

        public virtual Account Accounts { get; set; }
    }
}
