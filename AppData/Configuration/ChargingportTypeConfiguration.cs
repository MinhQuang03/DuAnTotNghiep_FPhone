using AppData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace AppData.Configuration
{
    public class ChargingportTypeConfiguration : IEntityTypeConfiguration<ChargingportType>
    {
        public void Configure(EntityTypeBuilder<ChargingportType> builder)
        {
            builder.HasKey(p => p.Id);
        }
    }
}
