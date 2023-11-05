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
    public class CartDetailepository : ICartDetailRepository
    {
        public readonly FPhoneDbContext _dbContext;
        public CartDetailepository()
        {
                
        }
        public CartDetailepository(FPhoneDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<CartDetails> Add(CartDetails obj)
        {
            await _dbContext.CartsDetails.AddAsync(obj);
            await _dbContext.SaveChangesAsync();
            return obj;

        }

        public async Task Delete(Guid id)
        {
            var a = await _dbContext.CartsDetails.FindAsync(id);
            _dbContext.CartsDetails.Remove(a);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<CartDetails>> GetAll()
        {
            return await _dbContext.CartsDetails.ToListAsync();
        }

        public async Task<CartDetails> GetById(Guid id)
        {
            return await _dbContext.CartsDetails.FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<List<CartDetails>> GetByIdAcout(Guid id)
        {
            return await _dbContext.CartsDetails.Where(p => p.IdAccount == id).ToListAsync();
        }
        public async Task<CartDetails> Update(CartDetails obj)
        {
            var a = await _dbContext.CartsDetails.FindAsync(obj.Id);
            a.IdAccount = obj.IdAccount;
            a.IdPhoneDetaild = obj.IdPhoneDetaild;
            a.Quantity = obj.Quantity;
            a.Status = obj.Status;
            _dbContext.CartsDetails.Update(a);
            await _dbContext.SaveChangesAsync();
            return obj;
        }
    }
}