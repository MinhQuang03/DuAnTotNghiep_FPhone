using AppData.FPhoneDbContexts;
using AppData.IServices;
using AppData.Models;
using AppData.ViewModels.Options;
using AppData.ViewModels.Phones;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AppData.Services
{
    public class VwTop5PhoneService : IVwTop5PhoneServices
    {
        private FPhoneDbContext _dbContext;

        public VwTop5PhoneService(FPhoneDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<VTop5_PhoneSell>> listVwTop5PhoneGroup()
        {
           //var query = _dbContext.VW_Top5Phone.FromSqlRaw("select * from VW_Top5Phone").ToList();
           // var rs = new List<VTop5_PhoneSell>();
           return _dbContext.VW_Top5Phone.ToList();
        }


    }
}
