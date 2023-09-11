using AppData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace AppData.Configuration
{
    public class OperatingSystemConfiguration : IEntityTypeConfiguration<OperatingSystems>
    {
        public void Configure(EntityTypeBuilder<OperatingSystems> builder)
        {
            builder.HasKey(p => p.Id);
        }
    }
}
