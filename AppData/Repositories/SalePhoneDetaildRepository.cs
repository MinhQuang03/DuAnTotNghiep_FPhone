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
    public class SalePhoneDetaildRepository : ISalePhoneDetaildRepository
    {
        public readonly FPhoneDbContext _dbcontext;
        public SalePhoneDetaildRepository()
        {
            _dbcontext = new FPhoneDbContext();
        }

        public async Task<bool> Add(Guid idsale, Guid idphonedetaild)
        {
            try
            {
                SalePhoneDetaild salePhoneDt = new SalePhoneDetaild()
                {
                    Id = Guid.NewGuid(),
                    IdSales = idsale,
                    IdPhoneDetaild = idphonedetaild
                };
                await _dbcontext.AddAsync(salePhoneDt);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public Task Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<SalePhoneDetaild>> GetAll()
        {
            return await _dbcontext.SalePhoneDetailds.ToListAsync();
        }

        public Task<bool> Update(Guid id, Guid idsale, Guid idphonedetaild)
        {
            throw new NotImplementedException();
        }
    }
}
