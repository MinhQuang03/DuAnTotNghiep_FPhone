using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Models
{
    public class SellYearlys
    {
        public Guid Id { get; set; }

        public DateTime CreateTime { get; set; }
        public decimal? TotalMoneys { get; set; }
        public int? TotalQuantity { get; set; }

        public decimal? Refund { get; set; }
        public int? SellOnl { get; set; }


        public int? SellOff { get; set; }
        public string? BestSeller { get; set; }
        public string? Status { get; set; }
        [NotMapped]
        public int? Month {get; set; }
        [NotMapped]
        public int? Year {get; set; }

    }
}

