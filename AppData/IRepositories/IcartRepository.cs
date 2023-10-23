using AppData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.IRepositories
{
    public interface IcartRepository
    {
        Task<Cart> Add(Cart obj);
        Task<Cart> GetById(Guid id);
        Task<List<Cart>> GetAll();
        Task Delete(Guid id);

      
    }
}
