using AppData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace AppData.Configuration
{
    public class SellDailyConfiguration : IEntityTypeConfiguration<SellDailys>
    {
        public void Configure(EntityTypeBuilder<SellDailys> builder)
        {
            builder.HasKey(p => p.Id);
        }
    }
}
