using AppData.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace AppData.FPhoneDbContexts;

public class FPhoneDbContext : IdentityDbContext<ApplicationUser>
{
    public FPhoneDbContext()
    {
    }

    public FPhoneDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<Address> Address { get; set; }
    public DbSet<Battery> Battery { get; set; }
    public DbSet<Bill> Bill { get; set; }
    public DbSet<BillDetails> BillDetails { get; set; }
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartDetails> CartsDetails { get; set; }
    public DbSet<ChargingportType> ChargingportType { get; set; }
    public DbSet<ChipCPUs> ChipCPUs { get; set; }
    public DbSet<ChipGPUs> ChipGPUs { get; set; }
    public DbSet<Color> Colors { get; set; }
    public DbSet<Discount> Discount { get; set; }
    public DbSet<Imei> Imei { get; set; }
    public DbSet<ListImage> ListImage { get; set; }
    public DbSet<Material> Material { get; set; }
    public DbSet<OperatingSystems> OperatingSystem { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Phone> Phones { get; set; }
    public DbSet<PhoneDetaild> PhoneDetailds { get; set; }
    public DbSet<ProductionCompany> ProductionCompany { get; set; }
    public DbSet<Ram> Ram { get; set; }
    public DbSet<Rank> Ranks { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Rom> Rom { get; set; }
    public DbSet<Sim> Sim { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Warranty> Warranty { get; set; }
    public DbSet<WarrantyCard> WarrantyCards { get; set; }
    public DbSet<BillPhoneDetail> BillPhoneDetails { get; set; }
    public DbSet<ApplicationUser> AspNetUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Data Source=MSI;Initial Catalog=PRO219_WebsiteBanDienThoai;Integrated Security=True;TrustServerCertificate=True");
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Cart>().HasOne(c => c.Accounts).WithOne(p => p.Carts).HasForeignKey<Cart>();
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}