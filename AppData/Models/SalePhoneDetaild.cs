using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Models
{
    public class SalePhoneDetaild
    {
        public Guid Id { get; set; }

        public Guid IdSales { get; set; }

        public Guid IdPhoneDetaild { get; set; }

        public int Status { get; set; }

        public virtual PhoneDetaild? PhoneDetaild { get; set; }

        public virtual Sales? Sales { get; set; }
    }
}
