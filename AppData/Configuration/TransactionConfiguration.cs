using AppData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace AppData.Configuration
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(p => p.Id);

            builder.HasOne(p => p.Payments).WithMany().HasForeignKey(p => p.IdPayment);

            builder.HasOne(p => p.Bills).WithMany().HasForeignKey(p => p.IdBill);
        }
    }
}
