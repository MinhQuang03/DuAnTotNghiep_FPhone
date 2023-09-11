using AppData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace AppData.Configuration
{
    public class BatteryConfiguration : IEntityTypeConfiguration<Battery>
    {
        public void Configure(EntityTypeBuilder<Battery> builder)
        {
            builder.HasKey(p => p.Id);
        }
    }
}
