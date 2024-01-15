namespace AppData.Models
{
    public class Review
    {
        public Guid Id { get; set; }

        public DateTime DateTime { get; set; }

        public string? Content { get; set; }

        public Guid IdPhone { get; set; }

        public Guid IdAccount { get; set; }
    }
}
