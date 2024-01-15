using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModels.Phones
{
    public class VTop5_PhoneSell
    {
        public Guid Id { get; set; }
        public Guid IdPhone { get; set; }
        public string? PhoneName { get; set; }
        public decimal? Price { get; set;}
        public string?  Image { get; set; }

    }
}
