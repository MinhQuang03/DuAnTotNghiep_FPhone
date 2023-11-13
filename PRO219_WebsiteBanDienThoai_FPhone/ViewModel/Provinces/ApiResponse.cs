namespace PRO219_WebsiteBanDienThoai_FPhone.ViewModel.Provinces
{
    public class ApiResponse<T>
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public List<T> Data { get; set; }
    }
}
