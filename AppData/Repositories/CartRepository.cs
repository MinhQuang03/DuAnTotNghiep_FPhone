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
    public class CartRepository : IcartRepository
    {
        public readonly FPhoneDbContext _dbContext;
        public CartRepository()
        {
                
        }
        public CartRepository(FPhoneDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Cart> Add(Cart obj)
        {
            await _dbContext.Carts.AddAsync(obj);
            await _dbContext.SaveChangesAsync();
            return obj;
        }

        public async Task Delete(Guid id)
        {
            var a = await _dbContext.Carts.FindAsync(id);
            _dbContext.Carts.Remove(a);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Cart>> GetAll()
        {
            return await _dbContext.Carts.ToListAsync();
        }


        public async Task<Cart> GetById(Guid id)
        {
            return await _dbContext.Carts.FirstOrDefaultAsync(p => p.IdAccount == id);
        }

    }
}
