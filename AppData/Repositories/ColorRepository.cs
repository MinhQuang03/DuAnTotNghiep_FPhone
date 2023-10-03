using AppData.FPhoneDbContexts;
using AppData.IRepositories;
using AppData.Models;
using Microsoft.EntityFrameworkCore;

namespace AppData.Repositories
{
    public class ColorRepository : IColorRepository
    {
        public readonly FPhoneDbContext _dbContext;
        public ColorRepository(FPhoneDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Color?> Add(Color? obj)
        {
            await _dbContext.Colors.AddAsync(obj);
            await _dbContext.SaveChangesAsync();
            return obj;
        }

        public async Task Delete(Guid id)
        {
            var a = await _dbContext.Colors.FindAsync(id);
            _dbContext.Colors.Remove(a);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Color?>> GetAll()
        {
            return await _dbContext.Colors.ToListAsync();
        }

        public async Task<Color?> GetById(Guid id)
        {
            return await _dbContext.Colors.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Color> Update(Color obj)
        {
            var a = await _dbContext.Colors.FindAsync(obj.Id);
            a.Name = obj.Name;
            _dbContext.Colors.Update(a);
            await _dbContext.SaveChangesAsync();
            return obj;
        }
    }
}
