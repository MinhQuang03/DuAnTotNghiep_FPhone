using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModels.Phones
{
    [Serializable]
    [Table(name: "VW_PhoneDetail")]
    public class VW_PhoneDetail
    {
        [Key]
        public Guid IdPhoneDetail { get; set; }
        public Guid IdPhone { get; set; }
        public string PhoneName { get; set; }
        public string ReducedAmount { get; set; }
        public string MaterialName { get; set; }
        public string RomName { get; set; }
        public string RamName { get; set; }
        public string OperatingSystemName { get; set; }
        public string BatteryName { get; set; }
        public string SimName { get; set; }
        public string ChipCPUName { get; set; }
        public string ChipGPUName { get; set; }
        public string ColorName { get; set; }
        public string ChargingportTypeName { get; set; }
        public string Weight { get; set; }    
        public string FrontCamera { get; set; }    
        public string Resolution { get; set; }    
        public string Size { get; set; }    
        public decimal Price { get; set; }    
        public int Status { get; set; }    
    }
}
