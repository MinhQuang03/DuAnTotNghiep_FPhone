using AppData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace AppData.Configuration
{
    public class RomConfiguration : IEntityTypeConfiguration<Rom>
    {
        public void Configure(EntityTypeBuilder<Rom> builder)
        {
            builder.HasKey(p => p.Id);
        }
    }
}
