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
    public class MaterialRepository : IMaterialRepository
    {
        public readonly FPhoneDbContext _dbContext;

        public MaterialRepository()
        {
                _dbContext = new FPhoneDbContext();
        }
        public async Task<Material> Add(Material obj)
        {

            await _dbContext.Material.AddAsync(obj);
            await _dbContext.SaveChangesAsync();
            return obj;
        }

        public async Task Delete(Guid id)
        {
            var a = await _dbContext.Material.FindAsync(id);
            _dbContext.Material.Remove(a);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Material>> GetAll()
        {
            return await _dbContext.Material.ToListAsync();
        }

        public async Task<Material> GetById(Guid id)
        {
            return await _dbContext.Material.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Material> Update(Material obj)
        {
            var a = await _dbContext.Material.FindAsync(obj.Id);
            a.Name = obj.Name;
            _dbContext.Material.Update(a);
            await _dbContext.SaveChangesAsync();
            return obj;
        }
    }
}
