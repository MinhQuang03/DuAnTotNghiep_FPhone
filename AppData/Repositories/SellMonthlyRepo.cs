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
    public class SellMonthlyRepo : ISellMonthlyRepository
    {
        private FPhoneDbContext _dbContext;

        public SellMonthlyRepo(FPhoneDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<SellMonthlys>> GetByYear(int year)
        {
            return await _dbContext.SellMonthlys.Where(e => e.CreateTime.Year == year).ToListAsync();
        }


        public async Task<List<SellMonthlys>> GetAll()
        {
            return await _dbContext.SellMonthlys.ToListAsync();
        }


        public async Task<SellMonthlys> GetById(Guid id)
        {
            return await _dbContext.SellMonthlys.FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
