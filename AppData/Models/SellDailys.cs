using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Models
{
    public class SellDailys
    {
        public Guid Id { get; set; }

        public DateTime CreateTime { get; set; }
        public decimal? TotalMoneys { get; set; }
        public decimal? Refund { get; set; }
        public int? TotalQuantity { get; set; }
        public int? SellOnl { get; set; }
        public int? SellOff { get; set; }
        public string? BestSeller { get; set; }
        public string? Status { get; set; }

    }
}

