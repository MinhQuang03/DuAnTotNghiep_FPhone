using AppData.FPhoneDbContexts;
using AppData.IServices;
using AppData.ViewModels.Phones;

namespace AppData.Services
{
    public class VwPhoneService : IVwPhoneService
    {
        private FPhoneDbContext _dbContext;

        public VwPhoneService(FPhoneDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<VW_Phone_Group> listVwPhoneGroup()
        {
            
            var lst = new List<VW_Phone_Group>();
            try
            {
                lst = _dbContext.VW_Phone_Group.ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return lst;
        }
    }
}
