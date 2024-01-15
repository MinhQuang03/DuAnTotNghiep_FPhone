using AppData.IRepositories;
using AppData.Repositories;
using AppData.Services;
using AppData.IServices;

namespace AppApi
{
    public static class ServiceRegistration
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddTransient<IAccountsRepository, AccountsRepository>();
            services.AddTransient<ISimRepository, SimRepository>();
            services.AddTransient<IPhoneDetaildRepository, PhoneDetaildRepository>();
            services.AddTransient<IPhoneDetailService, PhoneDetailService>();
            services.AddTransient<IChargingportTypeRepository, ChargingportTypeRepository>();
            services.AddTransient<IChipCPURepository, ChipCPURepository>();
            services.AddTransient<IChipGPURepository, ChipGPURepository>();
            services.AddTransient<IColorRepository, ColorRepository>();
            services.AddTransient<IPhoneRepository, PhoneRepository>();
            services.AddTransient<IProductionCompanyRepository, ProductionCompanyRepository>();
            services.AddTransient<IBlogRepository,BlogRepository>();
            services.AddTransient<ICartDetailRepository,CartDetailepository>();
            services.AddTransient<IcartRepository, CartRepository>();
            services.AddTransient<IBatteryRepository, BatteryRepository>();
            services.AddTransient<IMaterialRepository, MaterialRepository>();
            services.AddTransient<IOpertingRepository, OperatingRepository>();
            services.AddTransient<IRamRepository, RamRepository>();
            services.AddTransient<IRomRepository, RomRepository>();
            services.AddTransient<ISimRepository, SimRepository>();
            services.AddTransient<IRanksRepositories, RankRepositories>();
            services.AddTransient<IBillRepository, BillRepository>();
            services.AddTransient<IImeiRepository, ImeiRepository>();
            services.AddTransient<IUserRepository, UserRepostitory>();
            services.AddTransient<IAddressRepository, AddressRepostitory>();
            services.AddTransient<IVwPhoneService, VwPhoneService>();
            services.AddTransient<IBillRepository, BillRepository>();
            services.AddTransient<ISaleRepository, SaleRepository>();
            services.AddTransient<ISalePhoneDetaildRepository, SalePhoneDetaildRepository>();
            services.AddTransient<ISellDailyRepository, SellDailyRepository>();
            services.AddTransient<ISellMonthlyRepository, SellMonthlyRepo>();
            services.AddTransient<ISellYearlyRepository, SellYearlyRepo>();
            services.AddTransient<IDiscountServices, DisocountRepository>();
        }
    }
}
