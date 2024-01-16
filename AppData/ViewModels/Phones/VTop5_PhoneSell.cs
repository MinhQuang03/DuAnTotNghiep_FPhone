using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModels.Phones
{
    public class VTop5_PhoneSell
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid IdPhone { get; set; }
        public string? PhoneName { get; set; }
        public decimal? Price { get; set;}
        public string?  Image { get; set; }
        [NotMapped]
        public int countPhone { get; set; } = 0;

    }
}
