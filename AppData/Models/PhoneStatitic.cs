using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Models
{
    public class PhoneStatitic
    {
        public Guid Id { get; set; }

        public DateTime CreateTime { get; set; }
        public string? Bestseller { get; set; }
        public int? SellerQuan { get; set; }
        public string? BestColor { get; set; }

        public int? ColorQuan { get; set; }
        public string? BestPriceRange { get; set; }

        public string? BuyTime { get; set; }
        public int? PecentPhone { get; set; }
        public int? TotalBackPhone { get; set; }
        public string? Note { get; set; }
        public string? Status { get; set; }
        [NotMapped]
        public int? Month {get; set; }
        [NotMapped]
        public int? Year {get; set; }

    }
}

