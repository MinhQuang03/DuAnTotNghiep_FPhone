using AppData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.IRepositories
{
    public interface ISalePhoneDetaildRepository
    {
        Task<bool> Add(Guid idsale, Guid idphonedetaild);
        Task<bool> Update(Guid id, Guid idsale, Guid idphonedetaild);
        Task<List<SalePhoneDetaild>> GetAll();
        Task Delete(Guid id);
    }
}
