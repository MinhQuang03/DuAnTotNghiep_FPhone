using AppData.FPhoneDbContexts;
using AppData.IRepositories;
using AppData.Models;
using Microsoft.EntityFrameworkCore;

namespace AppData.Repositories
{
    public class ChipCPURepository : IChipCPURepository
    {
        public readonly FPhoneDbContext _dbContext;
        public ChipCPURepository(FPhoneDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ChipCPUs> Add(ChipCPUs obj)
        {
            await _dbContext.ChipCPUs.AddAsync(obj);
            await _dbContext.SaveChangesAsync();
            return obj;
        }

        public async Task Delete(Guid id)
        {
            var a = await _dbContext.ChipCPUs.FindAsync(id);
            _dbContext.ChipCPUs.Remove(a);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<ChipCPUs>> GetAll()
        {
            return await _dbContext.ChipCPUs.ToListAsync();
        }

        public async Task<ChipCPUs> GetById(Guid id)
        {
            return await _dbContext.ChipCPUs.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<ChipCPUs> Update(ChipCPUs obj)
        {
            var a = await _dbContext.ChipCPUs.FindAsync(obj.Id);
            a.Name = obj.Name;
            _dbContext.ChipCPUs.Update(a);
            await _dbContext.SaveChangesAsync();
            return obj;
        }
    }
}
