namespace AppData.Models
{
    public class Blog
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string? Title { get; set; }
        public string IntroText { get; set; }

        public string? Content { get; set; }    

        public DateTime? CreatedDate { get; set; }

        public string? Images { get; set; }

        public int Status { get; set; }
    }
}
