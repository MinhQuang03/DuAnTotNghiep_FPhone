using AppData.FPhoneDbContexts;
using AppData.IRepositories;
using AppData.Models;
using Microsoft.EntityFrameworkCore;

namespace AppData.Repositories
{
    public class BatteryRepository : IBatteryRepository
    {
        public readonly FPhoneDbContext _dbContext;
        public BatteryRepository()
        {
            _dbContext = new FPhoneDbContext();
        }
        public async Task<Battery> Add(Battery obj)
        {
            
            await _dbContext.Battery.AddAsync(obj);
            await _dbContext.SaveChangesAsync();
            return obj;
        }

        public async Task Delete(Guid id)
        {
            var a = await _dbContext.Battery.FindAsync(id);
            _dbContext.Battery.Remove(a);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Battery>> GetAll()
        {
            return await _dbContext.Battery.ToListAsync();
        }

        public async Task<Battery> GetById(Guid id)
        {
            return await _dbContext.Battery.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Battery> Update(Battery obj)
        {
            var a = await _dbContext.Battery.FindAsync(obj.Id);
            a.Name = obj.Name;
            _dbContext.Battery.Update(a);
            await _dbContext.SaveChangesAsync();
            return obj;
        }
    }
}
