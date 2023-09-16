using AppData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.IRepositories
{
    public interface IImeiRepository
    {
        Task<Imei> Add(Imei obj);
        Task<Imei> Update(Imei obj);
        Task<List<Imei>> GetAll();
        Task Delete(Guid id);

        Task<Imei> GetById(Guid id);
    }
}
