namespace PRO219_WebsiteBanDienThoai_FPhone.Models
{
    public class BlogView
    {
        public Guid Id { get; set; }

        public string? Title { get; set; }

        public string? Content { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int Status { get; set; }
    }
}
