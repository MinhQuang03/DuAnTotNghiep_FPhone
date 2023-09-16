﻿using AppData.IRepositories;
using AppData.Repositories;

namespace AppApi
{
    public static class ServiceRegistration
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddScoped<IAccountStaffRepository, AccountStaffRepository>();
            services.AddScoped<ISimRepository, SimRepository>();
            services.AddScoped<IPhoneDetaildRepository, PhoneDetaildRepository>();
            services.AddScoped<IChargingportTypeRepository, ChargingportTypeRepository>();
            services.AddScoped<IChipCPURepository, ChipCPURepository>();
            services.AddScoped<IChipGPURepository, ChipGPURepository>();
            services.AddScoped<IColorRepository, ColorRepository>();
            services.AddScoped<IPhoneRepository, PhoneRepository>();
            services.AddScoped<IProductionCompanyRepository, ProductionCompanyRepository>();

            services.AddScoped<IBatteryRepository, BatteryRepository>();
            services.AddScoped<IMaterialRepository, MaterialRepository>();
            services.AddScoped<IOpertingRepository, OperatingRepository>();
            services.AddScoped<IRamRepository, RamRepository>();
            services.AddScoped<IRomRepository, RomRepository>();
            services.AddScoped<ISimRepository, SimRepository>();
            services.AddScoped<IImeiRepository, ImeiRepository>();
        }
    }
}
