using AppData.FPhoneDbContexts;
using AppData.IRepositories;
using AppData.Models;
using Microsoft.EntityFrameworkCore;

namespace AppData.Repositories
{
    public class ProductionCompanyRepository : IProductionCompanyRepository
    {
        public readonly FPhoneDbContext _dbContext;
        public ProductionCompanyRepository(FPhoneDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ProductionCompany> Add(ProductionCompany obj)
        {
            await _dbContext.ProductionCompany.AddAsync(obj);
            await _dbContext.SaveChangesAsync();
            return obj;
        }

        public async Task Delete(Guid id)
        {
            var a = await _dbContext.ProductionCompany.FindAsync(id);
            _dbContext.ProductionCompany.Remove(a);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<ProductionCompany>> GetAll()
        {
            return await _dbContext.ProductionCompany.ToListAsync();
        }

        public async Task<ProductionCompany> GetById(Guid id)
        {
            return await _dbContext.ProductionCompany.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<ProductionCompany> Update(ProductionCompany obj)
        {
            var a = await _dbContext.ProductionCompany.FindAsync(obj.Id);
            a.Name = obj.Name;
            _dbContext.ProductionCompany.Update(a);
            await _dbContext.SaveChangesAsync();
            return obj;
        }
    }
}
