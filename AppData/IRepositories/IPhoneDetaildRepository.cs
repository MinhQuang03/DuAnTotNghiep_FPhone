using AppData.Models;
using System.Dynamic;

namespace AppData.IRepositories
{
    public interface IPhoneDetaildRepository
    {
        Task<PhoneDetaild> Add(PhoneDetaild obj);
        Task<PhoneDetaild> Update(PhoneDetaild obj);

        Task<List<PhoneDetaild>> GetAll();
        Task<List<PhoneDetaild>> GetAll(Guid IdPhone);

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
