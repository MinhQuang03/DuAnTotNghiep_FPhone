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
    public class ListImageRepository : IListImgaeRepository
    {
        public readonly FPhoneDbContext _dbContext;
        public ListImageRepository()
        {
            _dbContext = new FPhoneDbContext();
        }
        public async Task<ListImage> Add(ListImage obj)
        {
            await _dbContext.ListImage.AddAsync(obj);
            await _dbContext.SaveChangesAsync();
            return obj;
        }

        public async Task Delete(Guid id)
        {
            var a = await _dbContext.ListImage.FindAsync(id);
            _dbContext.ListImage.Remove(a);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<ListImage>> GetAll()
        {
            return await _dbContext.ListImage.ToListAsync();
        }

        public  async Task<ListImage> GetById(Guid id)
        {
            return await _dbContext.ListImage.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<ListImage> Update(ListImage obj)
        {
            var a = await _dbContext.ListImage.FindAsync(obj.Id);
            a.PhoneDetailds = obj.PhoneDetailds;
            a.Image = obj.Image;
            a.IdPhoneDetaild = obj.IdPhoneDetaild;
            _dbContext.ListImage.Update(a);
            await _dbContext.SaveChangesAsync();
            return obj;
        }
    }
}
