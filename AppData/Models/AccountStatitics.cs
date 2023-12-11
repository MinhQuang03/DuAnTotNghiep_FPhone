using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Models
{
    public class AccountStatitics
    {
        public Guid Id { get; set; }

        public DateTime DateModified { get; set; }
        public int? TotalAccount { get; set; }
        public int? NewAccount { get; set; }
        public int? VVIP { get; set; }
        public int? Diamond { get; set; }
        public int? Gold { get; set; }
        public int? Silver { get; set; }
        public int? Bzone { get; set; }
        public int Blacklist { get; set; }
        public string? Status { get; set; }

    }
}

