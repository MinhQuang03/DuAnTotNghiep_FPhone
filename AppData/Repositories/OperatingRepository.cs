using AppData.FPhoneDbContexts;
using AppData.IRepositories;
using AppData.Models;
using Microsoft.EntityFrameworkCore;

namespace AppData.Repositories
{
    public class OperatingRepository : IOpertingRepository
    {
        public readonly FPhoneDbContext _dbContext;

        public OperatingRepository()
        {
                _dbContext = new FPhoneDbContext();
        }
        public async Task<OperatingSystems> Add(OperatingSystems obj)
        {
            
            await _dbContext.OperatingSystem.AddAsync(obj);
            await _dbContext.SaveChangesAsync();
            return obj;
            
        }

        public async Task Delete(Guid id)
        {
            var a = await _dbContext.OperatingSystem.FindAsync(id);
            _dbContext.OperatingSystem.Remove(a);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<OperatingSystems>> GetAll()
        {
            return await _dbContext.OperatingSystem.ToListAsync();
        }

        public async Task<OperatingSystems> GetById(Guid id)
        {
            return await _dbContext.OperatingSystem.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<OperatingSystems> Update(OperatingSystems obj)
        {
            var a = await _dbContext.OperatingSystem.FindAsync(obj.Id);
            a.Name = obj.Name;
            _dbContext.OperatingSystem.Update(a);
            await _dbContext.SaveChangesAsync();
            return obj;
        }
    }
}
