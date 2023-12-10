using AppData.Models;

namespace PRO219_WebsiteBanDienThoai_FPhone.ViewModel
{
    public class ProductListViewModel
    {

        /// <summary>
        ///     giá trị tìm kiếm
        /// </summary>
        /// <value></value>
        public Phone SearchData { get; set; } = new Phone();

        /// <summary>
        ///     danh sách bản ghi
        /// </summary>
        /// <value></value>
        public List<Phone> Records { get; set; } = new List<Phone>();

        /// <summary>
        ///     phân trang và sắp xếp
        /// </summary>
        /// <returns></returns>
        //public ListOptions ListOptions { get; set; } = new ListOptions();
    }

    public class ProductView
    {
        public Guid IdPhoneDetail { get; set; }
        public Guid IdProduct { get; set; }
        public string ProductName { get; set; }
        public string Price { get; set; }
        public string Image { get; set; }
    }
}
