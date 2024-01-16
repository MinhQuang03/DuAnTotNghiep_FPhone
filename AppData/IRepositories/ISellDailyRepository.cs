using AppData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.IRepositories
{
    public interface ISellDailyRepository
    {
        Task<List<SellDailys>> GetAll();
        Task<SellDailys> GetById(Guid id);
        Task<List<SellDailys>> GetByYear(int year);
        Task<List<SellDailys>> GetByMonth(int month);
    }
}
