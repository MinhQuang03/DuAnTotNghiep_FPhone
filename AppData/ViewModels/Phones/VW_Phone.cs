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
    [Table(name: "VW_Phone")]
    public class VW_Phone
    {
     
        public Guid IdPhone { get; set; }
        public string PhoneName { get; set; }
        public string Image { get; set; }
        public Guid IdProductionComany { get; set; }
        public string ProductionComanyName { get; set; }
    }
}
