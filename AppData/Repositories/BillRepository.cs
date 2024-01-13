using AppData.FPhoneDbContexts;
using AppData.IRepositories;
using AppData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Utilities;

namespace AppData.Repositories
{
    public class BillRepository :IBillRepository
    {
        public readonly FPhoneDbContext _dbContext;
        public BillRepository()
        {
            _dbContext = new FPhoneDbContext();
        }
        public async Task<Bill> Add(Bill obj)
        {
            Bill dbo = new Bill();
            try
            {
                BeanUtils.CopyAllPropertySameName(obj,dbo);
                _dbContext.Bill.Add(obj);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                dbo = null;
            }
          
            return dbo;
        }

        public async Task Delete(Guid id)
        {
            var a = await _dbContext.Bill.FindAsync(id);
            _dbContext.Bill.Remove(a);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Bill>> GetAll()
        {
            return await _dbContext.Bill.ToListAsync();
        }

        public async Task<Bill> GetById(Guid id)
        {
            return await _dbContext.Bill.FirstOrDefaultAsync(p => p.Id == id);
        }

        public BillDetails AddBillDetail(BillDetails model)
        {
            var data = new BillDetails();
            try
            {
                BeanUtils.CopyAllPropertySameName(model,data);
                _dbContext.BillDetails.Add(data);
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                data = null;
            }

            return data;
        }

        public async Task<Bill> Update(Bill obj)
        {
            var a = await _dbContext.Bill.FindAsync(obj.Id);
            a.CreatedTime = obj.CreatedTime;
            a.PaymentDate = obj.PaymentDate;
            a.Name = obj.Name;
            a.Address= obj.Address;
            a.Phone = obj.Phone;
            a.Status = obj.Status;
            a.IdAccount = obj.IdAccount;
            _dbContext.Bill.Update(a);
            await _dbContext.SaveChangesAsync();
            return obj;
        }
    }
}
