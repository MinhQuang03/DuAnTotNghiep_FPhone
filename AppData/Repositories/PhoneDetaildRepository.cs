using AppData.FPhoneDbContexts;
using AppData.IRepositories;
using AppData.Models;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace AppData.Repositories
{
    public class PhoneDetaildRepository : IPhoneDetaildRepository
    {
        public readonly FPhoneDbContext _dbContext;
        public PhoneDetaildRepository(FPhoneDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<PhoneDetaild> Add(PhoneDetaild obj)
        {
            await _dbContext.PhoneDetailds.AddAsync(obj);
            await _dbContext.SaveChangesAsync();
            return obj;
        }

        public async Task<List<PhoneDetaild>> GetAll()
        {
            return await _dbContext.PhoneDetailds.ToListAsync();
        }
        public async Task<List<PhoneDetaild>> GetAll(Guid IdPhone)
        {
            return await _dbContext.PhoneDetailds.Where(c =>c.Id == IdPhone).ToListAsync();
        }

        public async Task<PhoneDetaild> GetById(Guid id)
        {
            return await _dbContext.PhoneDetailds.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<PhoneDetaild> GetPhoneByChipCPUs(Guid IdChipCPUs)
        {
            return await _dbContext.PhoneDetailds.FirstOrDefaultAsync(p => p.IdChipCPU == IdChipCPUs);
        }

        public async Task<PhoneDetaild> GetPhoneByColor(Guid IdColor)
        {
            return await _dbContext.PhoneDetailds.FirstOrDefaultAsync(p => p.IdColor == IdColor);
        }

        public async Task<PhoneDetaild> GetPhoneByRam(Guid IdRam)
        {
            return await _dbContext.PhoneDetailds.FirstOrDefaultAsync(p => p.IdRam == IdRam);
        }

        public async Task<List<PhoneDetaild>> GetPhoneASC()
        {
            return await _dbContext.PhoneDetailds.OrderBy(p => p.Price).ToListAsync();
        }

        public async Task<List<PhoneDetaild>> GetPhoneDESC()
        {
            return await _dbContext.PhoneDetailds.OrderByDescending(p => p.Price).ToListAsync();
        }

        public async Task<PhoneDetaild> Update(PhoneDetaild obj)
        {
            var a = await _dbContext.PhoneDetailds.FindAsync(obj.Id);
            a.IdPhone = obj.IdPhone;
            a.IdDiscount = obj.IdDiscount;
            a.IdMaterial = obj.IdMaterial;
            a.IdRom = obj.IdRom;
            a.IdRam = obj.IdRam;
            a.IdOperatingSystem = obj.IdOperatingSystem;
            a.IdBattery = obj.IdBattery;
            a.IdSim = obj.IdSim;
            a.IdChipCPU = obj.IdChipCPU;
            a.IdChipGPU = obj.IdChipGPU;
            a.IdColor = obj.IdColor;
            a.IdChargingport = obj.IdChargingport;
            a.Weight = obj.Weight;
            a.FrontCamera = obj.FrontCamera;
            a.MainCamera = obj.MainCamera;
            a.Resolution = obj.Resolution;
            a.Size = obj.Size;
            a.Price = obj.Price;
            a.Status = obj.Status;
            _dbContext.PhoneDetailds.Update(a);
            await _dbContext.SaveChangesAsync();
            return obj;
        }

        public async Task<List<PhoneDetaild>> GetPhone5tr()
        {
            return await _dbContext.PhoneDetailds.Where(p => p.Price >= 500000 && p.Price <= 5000000).ToListAsync();
        }

        public async Task<List<PhoneDetaild>> GetPhone10tr()
        {
            return await _dbContext.PhoneDetailds.Where(p => p.Price >= 5000000 && p.Price <= 10000000).ToListAsync();
        }

        public async Task<List<PhoneDetaild>> GetPhone15tr()
        {
            return await _dbContext.PhoneDetailds.Where(p => p.Price >= 10000000 && p.Price <= 15000000).ToListAsync();
        }

        public async Task<List<PhoneDetaild>> GetPhone25tr()
        {
            return await _dbContext.PhoneDetailds.Where(p => p.Price >= 15000000 && p.Price <= 25000000).ToListAsync();
        }

        public async Task<List<PhoneDetaild>> GetPhone50tr()
        {
            return await _dbContext.PhoneDetailds.Where(p => p.Price >= 25000000 && p.Price <= 50000000).ToListAsync();
        }
    }
}
