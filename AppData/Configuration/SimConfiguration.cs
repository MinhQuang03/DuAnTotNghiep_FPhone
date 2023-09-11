using AppData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace AppData.Configuration
{
    public class SimConfiguration : IEntityTypeConfiguration<Sim>
    {
        public void Configure(EntityTypeBuilder<Sim> builder)
        {
            builder.HasKey(p => p.Id);
        }
    }
}
