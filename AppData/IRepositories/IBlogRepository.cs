using AppData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.IRepositories
{
    public interface IBlogRepository
    {
        Task<Blog> Add(Blog obj);
        Task<Blog> Update(Blog obj);
        Task<List<Blog>> GetAll();
        Task Delete(Guid id);

        Task<Blog> GetById(Guid id);
    }
}
