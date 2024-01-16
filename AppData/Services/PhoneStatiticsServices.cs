using AppData.ViewModels.ThongKe;
using AppData.FPhoneDbContexts;
using AppData.IServices;
using AppData.Models;
using AppData.ViewModels.Options;
using AppData.ViewModels.Phones;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AppData.Services
{
    public class PhoneStatiticsServices : IPhoneStatiticsServices
    {
        private FPhoneDbContext _dbContext;

        public PhoneStatiticsServices(FPhoneDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<PhoneStatitics> listPhoneStaticsGroup()
        {
            return _dbContext.PhoneStatitics.ToList();
        }
    }
}
