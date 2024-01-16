using AppData.FPhoneDbContexts;
using AppData.IServices;
using AppData.Models;
using AppData.ViewModels.Options;
using AppData.ViewModels.Phones;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AppData.Utilities;

namespace AppData.Services
{
    public class VwTop5PhoneService : IVwTop5PhoneServices
    {
        private FPhoneDbContext _dbContext;

        public VwTop5PhoneService(FPhoneDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<VTop5_PhoneSell> listVwTop5PhoneGroup()
        {
            var data = new List<VTop5_PhoneSell>();
            try
            {
                data = _dbContext.VTop5_PhoneSell.ToList();
                foreach (var item in data)
                {
                    item.countPhone = _dbContext.Imei.Count(c =>c.IdPhoneDetaild == item.Id && c.Status==1);
                }
            }
            catch (Exception e)
            {
               
            }

            return data;
        }

    }
}
