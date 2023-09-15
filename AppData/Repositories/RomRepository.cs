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
    public class RomRepository : IRomRepository
    {
        public readonly FPhoneDbContext _dbContext;
        public RomRepository()
        {
                _dbContext = new FPhoneDbContext();
        }
        public async Task<Rom> Add(Rom obj)
        {
            await _dbContext.Rom.AddAsync(obj);
            await _dbContext.SaveChangesAsync();
            return obj;
        }

        public async Task Delete(Guid id)
        {
            var a = await _dbContext.Rom.FindAsync(id);
            _dbContext.Rom.Remove(a);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Rom>> GetAll()
        {
            return await _dbContext.Rom.ToListAsync();
        }

        public async Task<Rom> GetById(Guid id)
        {
            return await _dbContext.Rom.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Rom> Update(Rom obj)
        {
            var a = await _dbContext.Rom.FindAsync(obj.Id);
            a.Name = obj.Name;
            _dbContext.Rom.Update(a);
            await _dbContext.SaveChangesAsync();
            return obj;
        }
    }
}
