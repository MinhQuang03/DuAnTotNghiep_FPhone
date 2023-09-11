using AppData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace AppData.Configuration
{
    public class PhoneDetaildConfiguration : IEntityTypeConfiguration<PhoneDetaild>
    {
        public void Configure(EntityTypeBuilder<PhoneDetaild> builder)
        {
            builder.HasKey(p => p.Id);

            builder.HasOne(p => p.Rams).WithMany().HasForeignKey(p => p.IdRam);

            builder.HasOne(p => p.Materials).WithMany().HasForeignKey(p => p.IdMaterial);

            builder.HasOne(p => p.Roms).WithMany().HasForeignKey(p => p.IdRom);

            builder.HasOne(p => p.OperatingSystems).WithMany().HasForeignKey(p => p.IdOperatingSystem);

            builder.HasOne(p => p.Batteries).WithMany().HasForeignKey(p => p.IdBattery);

            builder.HasOne(p => p.Sims).WithMany().HasForeignKey(p => p.IdSim);

            builder.HasOne(p => p.ChargingportTypes).WithMany().HasForeignKey(p => p.IdChargingport);

            builder.HasOne(p => p.Phones).WithMany().HasForeignKey(p => p.IdPhone);

            builder.HasOne(p => p.ChipCPUs).WithMany().HasForeignKey(p => p.IdChipCPU);

            builder.HasOne(p => p.ChipGPUs).WithMany().HasForeignKey(p => p.IdChipGPU);

            builder.HasOne(p => p.Colors).WithMany().HasForeignKey(p => p.IdColor);

            builder.HasOne(p => p.Discounts).WithMany().HasForeignKey(p => p.IdDiscount);
        }
    }
}
