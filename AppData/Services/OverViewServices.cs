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
    public class OverViewServices : IvOverViewServices
    {
        private FPhoneDbContext _dbContext;

        public OverViewServices(FPhoneDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<vOverView> listOverViewGroup()
        {
            return _dbContext.OverViews.ToList();
        }


    }
}
