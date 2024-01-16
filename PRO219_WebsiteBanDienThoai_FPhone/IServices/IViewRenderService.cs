namespace PRO219_WebsiteBanDienThoai_FPhone.IServices
{
    public interface IViewRenderService
    {
        Task<string> RenderToStringAsync(string viewName, object model);
        Task<string> RenderToStringAsync(string viewName);
    }
}
