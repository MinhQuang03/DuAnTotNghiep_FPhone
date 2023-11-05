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

        public BlogViewComponents(HttpClient client)
        {
            _client = client;
        }
        public async Task<IViewComponentResult> InvokeAsync()   
        {
            var datajson = await _client.GetStringAsync("api/Blog/get");
            var blog = JsonConvert.DeserializeObject<List<Blog>>(datajson);

            var lstblogView = from a in blog
                              group a by new
                              {
                                  a.Id,
                                  a.Title,
                                  a.CreatedDate,
                                  a.Content
                              }
                into b
                              select new BlogView
                              {
                                  Id = b.Key.Id,
                                  Title = b.Key.Title,
                                  CreatedDate = b.Key.CreatedDate,
                                  Content = b.Key.Content
                              };
           
            return View(lstblogView.ToList());

        }
    }
}
