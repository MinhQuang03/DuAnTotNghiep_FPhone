using AppData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.ViewModels.Phones;

namespace AppData.IServices
{
    public interface IPhoneDetailService
    {
        Task<PhoneDetaild> Add(PhoneDetaild obj);
        Task<PhoneDetaild> Update(PhoneDetaild obj);
        public Task<List<PhoneDetaild>> GetPhoneDetailds();
        public Task<List<PhoneDetaild>> GetPhoneDetailds(Guid IdPhone);

        Task<PhoneDetaild> GetById(Guid id);

        Task<PhoneDetaild> GetPhoneByColor(Guid IdColor);

        Task<PhoneDetaild> GetPhoneByRam(Guid IdRam);

        Task<PhoneDetaild> GetPhoneByChipCPUs(Guid IdChipCPUs);

        Task<List<PhoneDetaild>> GetPhoneDESC();

        Task<List<PhoneDetaild>> GetPhoneASC();

        Task<List<PhoneDetaild>> GetPhone5tr();

        Task<List<PhoneDetaild>> GetPhone10tr();

        Task<List<PhoneDetaild>> GetPhone15tr();

        Task<List<PhoneDetaild>> GetPhone25tr();

        Task<List<PhoneDetaild>> GetPhone50tr();
       
    }
}
