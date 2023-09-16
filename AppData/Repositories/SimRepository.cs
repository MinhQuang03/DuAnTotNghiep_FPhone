using AppData.FPhoneDbContexts;
using AppData.IRepositories;
using AppData.Models;
using Microsoft.EntityFrameworkCore;

namespace AppData.Repositories
{
    public class SimRepository : ISimRepository
    {
        private FPhoneDbContext _dbContext;
        public SimRepository(FPhoneDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Sim> Add(Sim obj)
        {
            await _dbContext.Sim.AddAsync(obj);
            await _dbContext.SaveChangesAsync();
            return obj;
        }

        public async Task Delete(Guid id)
        {
            var a = await _dbContext.Sim.FindAsync(id);
            _dbContext.Sim.Remove(a);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Sim>> GetAll()
        {
            return await _dbContext.Sim.ToListAsync();
        }

        public async Task<Sim> GetById(Guid id)
        {
            return await _dbContext.Sim.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Sim> Update(Sim obj)
        {
            var a = await _dbContext.Sim.FindAsync(obj.Id);
            a.Name = obj.Name;
            _dbContext.Sim.Update(a);
            await _dbContext.SaveChangesAsync();
            return obj;
        }
    }
}
