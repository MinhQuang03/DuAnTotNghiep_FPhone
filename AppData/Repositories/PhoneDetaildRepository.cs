using AppData.FPhoneDbContexts;
using AppData.IRepositories;
using AppData.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task<PhoneDetaild> GetById(Guid id)
        {
            return await _dbContext.PhoneDetailds.FirstOrDefaultAsync(p => p.Id == id);
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
    }
}
