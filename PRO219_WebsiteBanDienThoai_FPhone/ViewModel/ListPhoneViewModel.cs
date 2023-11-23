using AppData.Models;
using AppData.ViewModels.Options;
using AppData.ViewModels.Phones;

namespace PRO219_WebsiteBanDienThoai_FPhone.ViewModel
{
	public class ListPhoneViewModel
	{
        public ListOptions Options { get; set; } = new ListOptions();
        public VW_PhoneDetail SearchData { get; set; } = new VW_PhoneDetail();
        public List<VW_Phone_Group> listVwPhoneGroup { get; set; } = new List<VW_Phone_Group>();
        public List<VW_PhoneDetail> ListvVwPhoneDetails { get; set; } = new List<VW_PhoneDetail>();
		public List<ProductionCompany> Brand { get; set; } = new List<ProductionCompany>();
		public List<Ram> listRam { get; set; } = new List<Ram>();
		public List<ChipCPUs> listChipCPU { get; set; } = new List<ChipCPUs>();
		public List<Rom> listRom { get; set; } = new List<Rom>();
		public List<Material> listMaterial { get; set; } = new List<Material>();
	}
}
    