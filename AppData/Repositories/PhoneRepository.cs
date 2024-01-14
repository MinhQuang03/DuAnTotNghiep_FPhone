using AppData.FPhoneDbContexts;
using AppData.IRepositories;
using AppData.Models;
using AppData.Utilities;
using AppData.ViewModels.Phones;
using Microsoft.EntityFrameworkCore;

namespace AppData.Repositories
{
    public class PhoneRepository : IPhoneRepository
    {
        public readonly FPhoneDbContext _dbContext;
        public PhoneRepository(FPhoneDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Phone> Add(Phone obj)
        {
            await _dbContext.Phones.AddAsync(obj);
            await _dbContext.SaveChangesAsync();
            return obj;
        }

        public async Task Delete(Guid id)
        {
            var a = await _dbContext.Phones.FindAsync(id);
            _dbContext.Phones.Remove(a);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Phone>> GetAll()
        {
            return await _dbContext.Phones.ToListAsync();
        }

        public async Task<Phone> GetById(Guid id)
        {
            return await _dbContext.Phones.FirstOrDefaultAsync(p => p.Id == id);
        }

        public int CheckExitPhone(string phoneName)
        {
            int phone = 0;
            try
            {
                phone = _dbContext.Phones.Count(c => c.PhoneName.ToLower() == phoneName.ToLower());
            }
            catch (Exception e)
            {
                
            }

            return phone;
        }


        public async Task<Phone> Update(Phone obj)
        {
            var a = await _dbContext.Phones.FindAsync(obj.Id);
            BeanUtils.CopyAllPropertySameName(obj,a);
            _dbContext.Phones.Update(a);
            await _dbContext.SaveChangesAsync();
            return obj;
        }
    }
}
