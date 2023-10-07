using AppData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Configuration
{
    public class SalePhoneDetaildConfiguration : IEntityTypeConfiguration<SalePhoneDetaild>
    {
        public void Configure(EntityTypeBuilder<SalePhoneDetaild> builder)
        {
            builder.HasKey(p => p.Id);

            builder.HasOne(p => p.Sales).WithMany().HasForeignKey(p => p.IdSales);

            builder.HasOne(p => p.PhoneDetaild).WithMany().HasForeignKey(p => p.IdPhoneDetaild);
        }
    }
}
