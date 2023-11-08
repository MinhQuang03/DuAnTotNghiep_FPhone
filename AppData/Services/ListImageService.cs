using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.FPhoneDbContexts;
using AppData.IServices;
using AppData.Models;
using Microsoft.EntityFrameworkCore;

namespace AppData.Services
{
    public class ListImageService : IListImageService
    {
        private FPhoneDbContext _dbContext;

        public ListImageService(FPhoneDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<ListImage> GetListImagesByIdPhoneDetail(Guid IdPhoneDetail)
        {
            var lst = new List<ListImage>();
            try
            {
                lst = _dbContext.ListImage.AsNoTracking().Where(c => c.IdPhoneDetaild == IdPhoneDetail).ToList();
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
