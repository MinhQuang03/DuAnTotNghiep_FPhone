using AppData.FPhoneDbContexts;
using AppData.IRepositories;
using AppData.Models;
using Microsoft.EntityFrameworkCore;

namespace AppData.Repositories
{
    public class ImeiRepository : IImeiRepository
    {
        public readonly FPhoneDbContext _dbContext;
        public ImeiRepository(FPhoneDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Imei> Add(Imei obj)
        {
            await _dbContext.Imei.AddAsync(obj);
            await _dbContext.SaveChangesAsync();
            return obj;
        }

        public async Task Delete(Guid id)
        {
            var a = await _dbContext.Imei.FindAsync(id);
            _dbContext.Imei.Remove(a);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Imei>> GetAll()
        {
            return await _dbContext.Imei.ToListAsync();
        }

        public async Task<Imei> GetById(Guid id)
        {
            return await _dbContext.Imei.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Imei> Update(Imei obj)
        {
            var a = await _dbContext.Imei.FindAsync(obj.Id);
            a.NameImei = obj.NameImei;
            a.Status = obj.Status;
            a.IdPhoneDetaild = obj.IdPhoneDetaild;
            _dbContext.Imei.Update(a);
            await _dbContext.SaveChangesAsync();
            return obj;
        }
    }
}
