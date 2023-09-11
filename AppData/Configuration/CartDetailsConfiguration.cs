using AppData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace AppData.Configuration
{
    public class CartDetailsConfiguration : IEntityTypeConfiguration<CartDetails>
    {
        public void Configure(EntityTypeBuilder<CartDetails> builder)
        {
            builder.HasKey(p => p.Id);

            builder.HasOne(p => p.PhoneDetailds).WithMany().HasForeignKey(p => p.IdPhoneDetaild);

            builder.HasOne(p => p.Carts).WithMany().HasForeignKey(p => p.IdAccount);
        }
    }
}
