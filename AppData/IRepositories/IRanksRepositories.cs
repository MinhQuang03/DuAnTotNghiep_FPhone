using AppData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.IRepositories
{
    public interface IRanksRepositories
    {
        Task<List<Rank>> GetAll();
    
        Task<Rank> GetById(Guid id);
        Task<List<Rank>> GetByYear(int days);
    }
}
