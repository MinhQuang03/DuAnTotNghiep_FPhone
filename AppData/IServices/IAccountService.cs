using AppData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.ViewModels;
using AppData.ViewModels.Options;

namespace AppData.IServices
{
    public interface IAccountService
    {
        public List<ApplicationUser> GetAllAsync(ApplicationUser Search, ListOptions options);
        public ApplicationUser GetById(string id);
        public ApplicationUser Update(string id, ApplicationUser user,out DataError error);   
    }       
}
