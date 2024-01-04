using AppData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace AppData.Configuration
{
    public class PhoneConfiguration : IEntityTypeConfiguration<Phone>
    {
        public void Configure(EntityTypeBuilder<Phone> builder)
        {
            builder.HasKey(p => p.Id);

            builder.HasOne(p => p.ProductionCompanies).WithMany().HasForeignKey(p => p.IdProductionCompany);
            builder.HasOne(p => p.Warranty).WithMany().HasForeignKey(p => p.IdWarranty);
        }
    }
}
