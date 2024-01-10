using AppData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.IRepositories
{
    public interface ISellYearlyRepository
    {
        Task<List<SellYearlys>> GetAll();
        Task<SellYearlys> GetById(Guid id);
        Task<List<SellYearlys>> GetByYear(int year);
    }
}
