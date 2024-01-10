using AppData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.IRepositories
{
    public interface ISellMonthlyRepository
    {
        Task<List<SellMonthlys>> GetAll();
        Task<SellMonthlys> GetById(Guid id);
        Task<List<SellMonthlys>> GetByYear(int year);
    }
}
