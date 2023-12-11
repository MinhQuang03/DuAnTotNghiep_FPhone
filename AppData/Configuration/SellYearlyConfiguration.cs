using AppData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace AppData.Configuration
{
    public class SellYearlyConfiguration : IEntityTypeConfiguration<SellYearly>
    {
        public void Configure(EntityTypeBuilder<SellYearly> builder)
        {
            builder.HasKey(p => p.Id);
        }
    }
}
