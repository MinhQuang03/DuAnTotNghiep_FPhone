using AppData.FPhoneDbContexts;
using AppData.IRepositories;
using AppData.Models;
using Microsoft.EntityFrameworkCore;

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
    }
}
