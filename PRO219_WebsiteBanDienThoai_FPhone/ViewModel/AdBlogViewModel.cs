using AppData.Models;
using AppData.ViewModels.Options;

namespace PRO219_WebsiteBanDienThoai_FPhone.ViewModel
{
    public class AdBlogViewModel
    {
        public List<Blog> Records { get; set; } = new List<Blog>();
        public ListOptions ListOptions { get; set; } = new ListOptions();
        public Blog SearchData { get; set; } = new Blog();
        public Blog DetailModel { get; set; } = new Blog();
    }
}
