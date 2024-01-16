using System.ComponentModel.DataAnnotations.Schema;
using AppData.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using AppData.ViewModels.Phones;
using System.Reflection.Emit;
using AppData.ViewModels.ThongKe;


namespace AppData.FPhoneDbContexts;

public class FPhoneDbContext : IdentityDbContext<ApplicationUser>
{
    public FPhoneDbContext()
    {
    }

    public FPhoneDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Contact> Contact { get; set; } 
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Address> Address { get; set; }
    public DbSet<Battery> Battery { get; set; }
    public DbSet<Bill> Bill { get; set; }
    public DbSet<BillDetails> BillDetails { get; set; }
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartDetails> CartDetails { get; set; }
    public DbSet<ChargingportType> ChargingportType { get; set; }
    public DbSet<ChipCPUs> ChipCPUs { get; set; }
    public DbSet<ChipGPUs> ChipGPUs { get; set; }
    public DbSet<Color?> Colors { get; set; }
    public DbSet<Discount> Discount { get; set; }
    public DbSet<Imei> Imei { get; set; }
    public DbSet<ListImage> ListImage { get; set; }
    public DbSet<Material> Material { get; set; }
    public DbSet<OperatingSystems> OperatingSystem { get; set; }
    public DbSet<Phone> Phones { get; set; }
    public DbSet<PhoneDetaild> PhoneDetailds { get; set; }
    public DbSet<ProductionCompany> ProductionCompany { get; set; }
    public DbSet<Ram> Ram { get; set; }
    public DbSet<Rank> Ranks { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Rom> Rom { get; set; }
    public DbSet<Sim> Sim { get; set; }
    public DbSet<Warranty> Warranty { get; set; }
    public DbSet<WarrantyCard> WarrantyCards { get; set; }
    public DbSet<Sales> Sales { get; set; }
    public DbSet<SellMonthlys> SellMonthlys { get; set; }
    public DbSet<SellYearlys> SellYearlys { get; set; }
    public DbSet<SellDailys> SellDaily { get; set; }
    public DbSet<SalePhoneDetaild> SalePhoneDetailds { get; set; }
    public DbSet<SellDailys> SellDailys { get; set; }
    public DbSet<ApplicationUser> AspNetUsers { get; set; }
    public DbSet<VW_Phone> VW_Phone { get; set; }
    public DbSet<VTop5_PhoneSell> VW_Top5Phone { get; set; }
    public DbSet<VW_PhoneDetail> VW_PhoneDetail { get; set; }
    public DbSet<VW_Phone_Group> VW_Phone_Group { get; set; }
    public DbSet<VW_List_By_IdPhone> VW_List_By_IdPhone { get; set; }
    public DbSet<VTop5_PhoneSell> VTop5_PhoneSell { get; set; }
    public DbSet<vOverView> OverViews { get; set; }
    public DbSet<BillGanDay> billGanDays { get; set; }
    public DbSet<PhoneStatitics> PhoneStatitics { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Data Source=.;Initial Catalog=PRO219_WebsiteBanDienThoai ;Integrated Security=True");
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Cart>().HasOne(c => c.Accounts).WithOne(p => p.Carts).HasForeignKey<Cart>();
        builder.Entity<VW_Phone>().ToView("VW_Phone").HasNoKey();
        builder.Entity<VW_PhoneDetail>().ToView("VW_PhoneDetail").HasNoKey();
        builder.Entity<VW_Phone_Group>().ToView("VW_Phone_Group").HasNoKey();
        builder.Entity<VW_List_By_IdPhone>().ToView("VW_List_By_IdPhone").HasNoKey();
        builder.Entity<VTop5_PhoneSell>().ToView("VTop5_PhoneSell").HasNoKey();
        builder.Entity<vOverView>().ToView("vOverView").HasNoKey();
        builder.Entity<BillGanDay>().ToView("BillGanDay").HasNoKey();
        builder.Entity<PhoneStatitics>().ToView("PhoneStatitics").HasNoKey();
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}