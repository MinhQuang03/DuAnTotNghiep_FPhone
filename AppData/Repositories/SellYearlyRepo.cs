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
    public class SellYearlyRepo : ISellYearlyRepository
    {
        private FPhoneDbContext _dbContext;

        public SellYearlyRepo(FPhoneDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<SellYearlys>> GetAll()
        {
            return await _dbContext.SellYearlys.ToListAsync();
        }

        public async Task<SellYearlys> GetById(Guid id)
        {
            return await _dbContext.SellYearlys.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<SellYearlys>> GetByYear(int year)
        {
            return await _dbContext.SellYearlys.Where(e => e.CreateTime.Year == year).ToListAsync();
        }
    }
}
