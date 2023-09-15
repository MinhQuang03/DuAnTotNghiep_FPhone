using AppData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.IRepositories
{
    public interface IRomRepository
    {
        Task<Rom> Add(Rom obj);
        Task<Rom> Update(Rom obj);
        Task<List<Rom>> GetAll();
        Task Delete(Guid id);

        Task<Rom> GetById(Guid id);
    }
}
