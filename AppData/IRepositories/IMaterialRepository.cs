using AppData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.IRepositories
{
    public interface IMaterialRepository
    {

        Task<Material> Add(Material obj);
        Task<Material> Update(Material obj);
        Task<List<Material>> GetAll();
        Task Delete(Guid id);

        Task<Material> GetById(Guid id);
    }
}
