using AppData.Models;

namespace PRO219_WebsiteBanDienThoai_FPhone.Models
{
    public class ProductDetailView
    {
        public Guid IdProduct { get; set; }
        public Guid IdProductDetail { get; set; }   
        public string ProductName { get; set; }
        public decimal Price { get; set; }  
        public string Description { get; set; }
        public string Brand { get; set; }
        public string Image { get; set; }
        public List<Color> Color { get; set; }  
        public Ram Ram { get; set;}  
        public Rom Rom { get; set;}      
    }
}
