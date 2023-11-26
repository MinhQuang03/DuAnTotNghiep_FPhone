using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Models
{
    public class Sales
    {
        public Guid Id { get; set; }
        public decimal ReducedAmount { get; set; }

        public DateTime? TimeForm { get; set; }

        public DateTime? TimeTo { get; set; }

        public string? Note { get; set; }

        public int Status { get; set; }
    }
}
