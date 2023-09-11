using AppData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace AppData.Configuration
{
    public class WarrantyCardConfiguration : IEntityTypeConfiguration<WarrantyCard>
    {
        public void Configure(EntityTypeBuilder<WarrantyCard> builder)
        {
            builder.HasKey(p => p.Id);

            builder.HasOne(p => p.BillDetails).WithMany().HasForeignKey(p => p.IdBillDetail);
        }
    }
}
