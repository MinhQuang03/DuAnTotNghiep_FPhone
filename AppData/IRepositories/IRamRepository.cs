using AppData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.IRepositories
{
    public interface IRamRepository
    {
        Task<Ram> Add(Ram obj);
        Task<Ram> Update(Ram obj);
        Task<List<Ram>> GetAll();
        Task Delete(Guid id);

        Task<Ram> GetById(Guid id);
    }
}
