using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppData.ViewModels.Phones
{
    [Serializable]
    [Table(name: "VW_PhoneDetail")]
    public class VW_PhoneDetail
    {
       
        public Guid IdPhoneDetail { get; set; }
        public Guid IdPhone { get; set; }
        public Guid RamID { get; set; }
        public Guid ColorID { get; set; }
        public string? Code { get; set; }
        public string? PhoneName { get; set; }
        public decimal? ReducedAmount { get; set; }
        public string? MaterialName { get; set; }
        public string? RomName { get; set; }
        public string? RamName { get; set; }
        public string? OperatingSystemName { get; set; }
        public string? BatteryName { get; set; }
        public string? SimName { get; set; }
        public string? ChipCPUName { get; set; }
        public string? ChipGPUName { get; set; }
        public string? ColorName { get; set; }
        public string? ChargingportTypeName { get; set; }
        public string? Weight { get; set; }    
        public string? FrontCamera { get; set; }    
        public int? Resolution { get; set; }    
        public string? Size { get; set; }    
        public decimal? Price { get; set; }    
        public int? Status { get; set; }    
        public string? ProductionCompanyName { get; set; }
        [NotMapped]
        public string? FirstImage { get; set; }

        [NotMapped]
        public int CountPhone { get; set; }
        [NotMapped]
        public List<string> ListImage { get; set; } = new List<string>();

        [NotMapped] public int CountImei { get; set; } = 0;
    }
}
