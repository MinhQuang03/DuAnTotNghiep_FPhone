using AppData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace AppData.Configuration
{
    public class BillDetailPhoneDetailConfiguration : IEntityTypeConfiguration<BillPhoneDetail>
    {
        public void Configure(EntityTypeBuilder<BillPhoneDetail> builder)
        {
            builder.HasKey(c => new { c.BillDetailId, c.PhoneDetailId });
            builder.HasOne(c => c.BillDetails).WithMany().HasForeignKey(c => c.BillDetailId);
            builder.HasOne(c => c.PhoneDetaild).WithMany().HasForeignKey(d => d.PhoneDetailId);
        }
    }
}
