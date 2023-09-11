using AppData.FPhoneDbContexts;
using AppData.IRepositories;
using AppData.Models;
using Microsoft.EntityFrameworkCore;

namespace AppData.Repositories
{
    public class ChipGPURepository : IChipGPURepository
    {
        public readonly FPhoneDbContext _dbContext;
        public ChipGPURepository(FPhoneDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ChipGPUs> Add(ChipGPUs obj)
        {
            await _dbContext.ChipGPUs.AddAsync(obj);
            await _dbContext.SaveChangesAsync();
            return obj;
        }

        public async Task Delete(Guid id)
        {
            var a = await _dbContext.ChipGPUs.FindAsync(id);
            _dbContext.ChipGPUs.Remove(a);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<ChipGPUs>> GetAll()
        {
            return await _dbContext.ChipGPUs.ToListAsync();
        }

        public async Task<ChipGPUs> GetById(Guid id)
        {
            return await _dbContext.ChipGPUs.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<ChipGPUs> Update(ChipGPUs obj)
        {
            var a = await _dbContext.ChipGPUs.FindAsync(obj.Id);
            a.Name = obj.Name;
            _dbContext.ChipGPUs.Update(a);
            await _dbContext.SaveChangesAsync();
            return obj;
        }
    }
}
