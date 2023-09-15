using AppData.FPhoneDbContexts;
using AppData.IRepositories;
using AppData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Repositories
{
    public class RamRepository : IRamRepository
    {
        public readonly FPhoneDbContext _dbContext;

        public RamRepository()
        {
                _dbContext = new FPhoneDbContext();
        }
        public async Task<Ram> Add(Ram obj)
        {
            await _dbContext.Ram.AddAsync(obj);
            await _dbContext.SaveChangesAsync();
            return obj;
        }
        public async Task Delete(Guid id)
        {
            var a = await _dbContext.Ram.FindAsync(id);
            _dbContext.Ram.Remove(a);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Ram>> GetAll()
        {
            return await _dbContext.Ram.ToListAsync();
        }

        public async Task<Ram> GetById(Guid id)
        {
            return await _dbContext.Ram.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Ram> Update(Ram obj)
        {
            var a = await _dbContext.Ram.FindAsync(obj.Id);
            a.Name = obj.Name;
            _dbContext.Ram.Update(a);
            await _dbContext.SaveChangesAsync();
            return obj;
        }
    }
}
