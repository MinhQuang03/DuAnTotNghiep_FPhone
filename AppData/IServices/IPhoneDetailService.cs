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
        Task<PhoneDetaild> Add(PhoneDetaild obj);
        Task<PhoneDetaild> Update(PhoneDetaild obj);
        public Task<List<PhoneDetaild>> GetPhoneDetailds();
        public Task<List<PhoneDetaild>> GetPhoneDetailds(Guid IdPhone);

        Task<PhoneDetaild> GetById(Guid id);
    }
}
