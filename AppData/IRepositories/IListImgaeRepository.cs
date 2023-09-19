using AppData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.IRepositories
{
    public interface IListImgaeRepository
    {
        Task<ListImage> Add(ListImage obj);
        Task<ListImage> Update(ListImage obj);
        Task<List<ListImage>> GetAll();
        Task Delete(Guid id);
        Task<ListImage> GetById(Guid id);
    }
}
