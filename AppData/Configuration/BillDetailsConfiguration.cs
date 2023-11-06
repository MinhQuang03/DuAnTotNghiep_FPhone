using AppData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace AppData.Configuration
{
    public class BillDetailsConfiguration : IEntityTypeConfiguration<BillDetails>
    {
        public void Configure(EntityTypeBuilder<BillDetails> builder)
        {
            builder.HasKey(p => p.Id);


            builder.HasOne(p => p.Discounts).WithMany().HasForeignKey(p => p.IdDiscount);

            builder.HasOne(p => p.Bills).WithMany().HasForeignKey(p => p.IdBill);
            builder.HasOne(p => p.PhoneDetaild).WithMany().HasForeignKey(p => p.IdPhoneDetail);
        }
    }
}
