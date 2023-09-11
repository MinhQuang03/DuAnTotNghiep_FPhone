using AppData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace AppData.Configuration
{
    public class ChipCPUConfiguration : IEntityTypeConfiguration<ChipCPUs>
    {
        public void Configure(EntityTypeBuilder<ChipCPUs> builder)
        {
            builder.HasKey(p => p.Id);
        }
    }
}
