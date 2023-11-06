using AppData.IRepositories;
using AppData.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PRO219_WebsiteBanDienThoai_FPhone.Models;

namespace PRO219_WebsiteBanDienThoai_FPhone.ViewComponents
{
    [ViewComponent(Name = "BlogView")]

    public class BlogViewComponents : ViewComponent
    {
        private readonly HttpClient _client;
        private IBlogRepository _blogRepository;
        public BlogViewComponents(HttpClient client,IBlogRepository blogRepository)
        {
            _client = client;
            _blogRepository = blogRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var data = await _blogRepository.GetAll();
           
            return View(data);

        }
    }
}
