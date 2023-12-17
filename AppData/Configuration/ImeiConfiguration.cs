using AppData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace AppData.Configuration
{
    public class ImeiConfiguration : IEntityTypeConfiguration<Imei>
    {
        public void Configure(EntityTypeBuilder<Imei> builder)
        {
            builder.HasKey(p => p.Id);

            builder.HasOne(p => p.PhoneDetaild).WithMany().HasForeignKey(p => p.IdPhoneDetaild);
        }
    }
}
