using AppData.ViewModels.Discount;
using AppData.FPhoneDbContexts;
using AppData.IServices;
using AppData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Repositories
{
    public class DisocountRepository : IDiscountServices
    {
        public readonly FPhoneDbContext _dbContext;
        public DisocountRepository(FPhoneDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> AddVoucher(Discount discount)
        {
            await _dbContext.Discount.AddAsync(discount);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteVoucher(Guid id)
        {
            var a = await _dbContext.Discount.FindAsync(id);
            _dbContext.Discount.Remove(a);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EditVoucher(Discount discount)
        {

            var a = await _dbContext.Discount.FindAsync(discount.Id);
            a.NameVoucher = discount.NameVoucher;
            a.MaVoucher = discount.MaVoucher;
            a.TypeVoucher = discount.TypeVoucher;
            a.Quantity = discount.Quantity;
            a.MucUuDai = discount.MucUuDai;
            a.StatusVoucher = discount?.StatusVoucher;
            a.DateEnd = discount.DateEnd;
            a.DateStart = discount.DateStart;
            a.DieuKien = discount?.DieuKien;
            _dbContext.Discount.Update(a);
            await _dbContext.SaveChangesAsync();
            return true;
        }

     

        public async Task<List<Discount>> GetallVoucher()
        {
            return await _dbContext.Discount.ToListAsync();
        }

        public List<Discount> GetallVoucher2()
        {
            var dis = new List<Discount>();
            try
            {
                dis = _dbContext.Discount.ToList();
            }
            catch (Exception ex)
            {

            }
            return dis;
        }

        //public Task<VoucherDTO> GetVoucherDTOById(string id)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
