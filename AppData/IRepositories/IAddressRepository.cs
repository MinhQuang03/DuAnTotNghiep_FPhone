using AppData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.IRepositories
{
    public interface IAddressRepository
    {
        public Task<Address?> GetAddress(Guid IdUser); 
    }   
}
