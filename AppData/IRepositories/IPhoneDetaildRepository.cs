using AppData.Models;

namespace AppData.IRepositories
{
    public interface IPhoneDetaildRepository
    {
        Task<PhoneDetaild> Add(PhoneDetaild obj);
        Task<PhoneDetaild> Update(PhoneDetaild obj);

        Task<List<PhoneDetaild>> GetAll();
        Task<List<PhoneDetaild>> GetAll(Guid IdPhone);

        Task<PhoneDetaild> GetById(Guid id);
    }
}
