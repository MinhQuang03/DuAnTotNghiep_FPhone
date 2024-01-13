using AppData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.IRepositories
{
    public interface IBillRepository
    {
        Task<Bill> Add(Bill obj);
        Task<Bill> Update(Bill obj);
        Task<List<Bill>> GetAll();
        Task Delete(Guid id);

        Task<Bill> GetById(Guid id);
        BillDetails AddBillDetail(BillDetails model);

    }
}
