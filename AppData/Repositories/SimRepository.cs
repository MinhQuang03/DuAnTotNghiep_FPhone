using AppData.FPhoneDbContexts;
using AppData.IRepositories;
using AppData.Models;

namespace AppData.Repositories
{
    public class SimRepository:ISimRepository
    {
        private FPhoneDbContext _dbContext;
        public SimRepository(FPhoneDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> Create(Sim obj)
        {
            try
            {
                _dbContext.Add(obj);
                 await _dbContext.SaveChangesAsync();
               return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}
