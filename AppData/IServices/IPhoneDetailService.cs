using AppData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.IServices
{
    public interface IPhoneDetailService
    {
        public Task<List<PhoneDetaild>> GetPhoneDetailds();
    }
}
