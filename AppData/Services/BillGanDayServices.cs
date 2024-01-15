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
    public class BillGanDayServices : IBillGanDayServices
    {
        private FPhoneDbContext _dbContext;

        public BillGanDayServices(FPhoneDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<BillGanDay> listBillGanDayViewGroup()
        {
            return _dbContext.billGanDays.ToList();
        }

      


    }
}
