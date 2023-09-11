using AppData.FPhoneDbContexts;
using AppData.IRepositories;
using AppData.Models;
using Microsoft.EntityFrameworkCore;

namespace AppData.Repositories
{
    public class ChargingportTypeRepository : IChargingportTypeRepository
    {
        public readonly FPhoneDbContext _dbContext;
        public ChargingportTypeRepository(FPhoneDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ChargingportType> Add(ChargingportType obj)
        {
            await _dbContext.ChargingportType.AddAsync(obj);
            await _dbContext.SaveChangesAsync();
            return obj;
        }

        public async Task Delete(Guid id)
        {
            var a = await _dbContext.ChargingportType.FindAsync(id);
            _dbContext.ChargingportType.Remove(a);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<ChargingportType>> GetAll()
        {
            return await _dbContext.ChargingportType.ToListAsync();
        }

        public async Task<ChargingportType> GetById(Guid id)
        {
            return await _dbContext.ChargingportType.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<ChargingportType> Update(ChargingportType obj)
        {
            var a = await _dbContext.ChargingportType.FindAsync(obj.Id);
            a.Name = obj.Name;
            _dbContext.ChargingportType.Update(a);
            await _dbContext.SaveChangesAsync();
            return obj;
        }
    }
}
