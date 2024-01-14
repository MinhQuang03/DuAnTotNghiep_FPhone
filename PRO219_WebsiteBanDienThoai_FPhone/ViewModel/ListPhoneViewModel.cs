using AppData.Models;
using AppData.ViewModels.Options;
using AppData.ViewModels.Phones;

namespace PRO219_WebsiteBanDienThoai_FPhone.ViewModel
{
	public class ListPhoneViewModel
	{
        public ListOptions Options { get; set; } = new ListOptions();
        public VW_PhoneDetail SearchData { get; set; } = new VW_PhoneDetail();
        public VW_PhoneDetail Record { get; set; } = new VW_PhoneDetail();
        public List<VW_PhoneDetail> ListvVwPhoneDetails { get; set; } = new List<VW_PhoneDetail>();
		public List<ProductionCompany> Brand { get; set; } = new List<ProductionCompany>();
		public List<Ram> listRam { get; set; } = new List<Ram>();
		public List<ChipCPUs> listChipCPU { get; set; } = new List<ChipCPUs>();
		public List<Rom> listRom { get; set; } = new List<Rom>();
		public List<Material> listMaterial { get; set; } = new List<Material>();
        public List<IFormFile> Images { get; set; } = new List<IFormFile>();
        public List<ListImage> listImage { get; set; } = new List<ListImage>();
        public List<VW_List_By_IdPhone> listImageByIdPhone { get; set; } = new List<VW_List_By_IdPhone>();
        public string FirstImage { get; set; }
        public Bill Bill { get; set; } = new Bill();
        public List<BillDetails> ListBillDetail { get; set; } = new();

    }
}
        