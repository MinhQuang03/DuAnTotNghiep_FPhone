using AppData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace AppData.Configuration
{
    public class SellMonthlyConfiguration : IEntityTypeConfiguration<SellMonthly>
    {
        public void Configure(EntityTypeBuilder<SellMonthly> builder)
        {
            builder.HasKey(p => p.Id);
        }
    }
}
