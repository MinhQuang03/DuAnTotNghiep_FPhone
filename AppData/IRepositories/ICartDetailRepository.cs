using AppData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.IRepositories
{
    public interface ICartDetailRepository
    {
        Task<CartDetails> Add(CartDetails obj);
        Task<CartDetails> Update(CartDetails obj);
        Task<List<CartDetails>> GetAll();
        Task Delete(Guid id);

        Task<CartDetails> GetById(Guid id);
        Task<List<CartDetails>> GetByIdAcout(Guid id);
    }
}
