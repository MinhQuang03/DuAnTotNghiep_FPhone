namespace PRO219_WebsiteBanDienThoai_FPhone.ViewModel
{
    public class BlogDetailView
    {
        public Guid IdBlog { get; set; }
        public Guid IdBlogDetail { get; set; }
        public string? Title { get; set; }

        public string? Content { get; set; }

        public string? Images { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
