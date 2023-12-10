namespace AppData.ViewModels
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
    public class DataError
    {
        /// <summary>
        /// thành công hay thất bại
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// thông báo
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// danh sách thông báo nếu có nhiều lỗi
        /// </summary>
        public List<string> MsgList { get; set; }
        public bool hasRight { get; set; }
        /// <summary>
        /// đối tượng trả về
        /// </summary>
        public object ReturnObj { get; set; }
    }
}