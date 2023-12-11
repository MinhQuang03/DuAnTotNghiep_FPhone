using AppData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace AppData.Configuration
{
    public class AccountStatitisConfiguration : IEntityTypeConfiguration<AccountStatitics>
    {
        public void Configure(EntityTypeBuilder<AccountStatitics> builder)
        {
            builder.HasKey(p => p.Id);
        }
    }
}
