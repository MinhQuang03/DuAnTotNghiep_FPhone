using AppData.FPhoneDbContexts;
using AppData.IRepositories;
using AppData.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace AppData.Repositories
{
    public class UserRepostitory : IUserRepository
    {
        private FPhoneDbContext _dbContexts;

        public UserRepostitory(FPhoneDbContext dbContexts)
        {
            _dbContexts = dbContexts;
        }
        public Task<List<Account>> GetAllAsync()
        {
           return _dbContexts.Accounts.AsNoTracking().ToListAsync();
        }

        public Task<Account?> GetById(Guid id)
        {
            return _dbContexts.Accounts.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Account> UpdateUser(Account account)
        {
            var result = _dbContexts.Accounts.FirstOrDefault(c => c.Id == account.Id);
            if (result != null)
            {
                result.Password = account.Password;
                result.Name = account.Name;
                result.Email = account.Email;
                result.PhoneNumber = account.PhoneNumber;
                result.ImageUrl = account.ImageUrl;
                result.Status = account.Status;
                result.Points = account.Points;

                await _dbContexts.SaveChangesAsync(); // Use asynchronous SaveChanges

                return result;
            }
            return null;
        }
    }
}
