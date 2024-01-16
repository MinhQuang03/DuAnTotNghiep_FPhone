using AppData.FPhoneDbContexts;
using AppData.IRepositories;
using AppData.Models;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Repositories
{
    public class SellDailyRepository : ISellDailyRepository
    {
        private FPhoneDbContext _dbContext;

        public SellDailyRepository(FPhoneDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<SellDailys>> GetAll()
        {
            return await _dbContext.SellDailys.ToListAsync();
        }

        public async Task<SellDailys> GetById(Guid id)
        {
            return await _dbContext.SellDailys.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<SellDailys>> GetByYear(int year)
        {
            return await _dbContext.SellDailys.Where(e => e.CreateTime.Year == year).ToListAsync();
        }

        public async Task<List<SellDailys>> GetByMonth(int month)
        {
            return await _dbContext.SellDailys.Where(e => e.CreateTime.Month == month).ToListAsync();
        }
    }
}
