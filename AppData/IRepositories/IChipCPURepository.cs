using AppData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.IRepositories
{
    public interface IChipCPURepository
    {
        Task<ChipCPUs> Add(ChipCPUs obj);
        Task<ChipCPUs> Update(ChipCPUs obj);
        Task<List<ChipCPUs>> GetAll();
        Task Delete(Guid id);

        Task<ChipCPUs> GetById(Guid id);
    }
}
