using AppData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.IRepositories
{
    public interface IOpertingRepository
    {
        Task<OperatingSystems> Add(OperatingSystems obj);
        Task<OperatingSystems> Update(OperatingSystems obj);
        Task<List<OperatingSystems>> GetAll();
        Task Delete(Guid id);

        Task<OperatingSystems> GetById(Guid id);
    }
}
