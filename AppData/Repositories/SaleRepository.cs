using AppData.FPhoneDbContexts;
using AppData.IRepositories;
using AppData.Models;
using Microsoft.EntityFrameworkCore;

namespace AppData.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        public readonly FPhoneDbContext _dbContetx;
        public SaleRepository()
        {
            _dbContetx = new FPhoneDbContext();
        }
        public async Task<Sales> Add(Sales obj)
        {
            obj.Id = new Guid();
            await _dbContetx.Sales.AddAsync(obj);
            await _dbContetx.SaveChangesAsync();
            return obj;
        }

        public async Task Delete(Guid id)
        {
            var a = await _dbContetx.Sales.FindAsync(id);
            _dbContetx.Sales.Remove(a);
            await _dbContetx.SaveChangesAsync();
        }

        public async Task<List<Sales>> GetAll()
        {
            return await _dbContetx.Sales.ToListAsync();
        }

        public async Task<Sales> Update(Sales obj)
        {
            var a = await _dbContetx.Sales.FindAsync(obj.Id);
            a.Status = obj.Status;
            a.TimeForm = obj.TimeForm;
            a.Note = obj.Note;
            a.TimeTo = obj.TimeTo;
            a.ReducedAmount = obj.ReducedAmount;
            _dbContetx.Sales.Update(a);
            await _dbContetx.SaveChangesAsync();
            return obj;
        }
    }
}
