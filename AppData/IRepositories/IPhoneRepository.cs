using AppData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.ViewModels.Phones;

namespace AppData.IRepositories
{
    public interface IPhoneRepository
    {
        Task<Phone> Add(Phone obj);
        Task<Phone> Update(Phone obj);
        Task<List<Phone>> GetAll();
        Task Delete(Guid id);
        Task<Phone> GetById(Guid id);
        int CheckExitPhone(string phoneName);
    }
}
