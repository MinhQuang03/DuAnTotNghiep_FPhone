using AppData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace AppData.Configuration
{
    public class ChipGPUConfiguration : IEntityTypeConfiguration<ChipGPUs>
    {
        public void Configure(EntityTypeBuilder<ChipGPUs> builder)
        {
            builder.HasKey(p => p.Id);
        }
    }
}
