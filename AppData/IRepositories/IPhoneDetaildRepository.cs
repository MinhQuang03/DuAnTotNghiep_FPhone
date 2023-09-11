using AppData.Models;

namespace AppData.IRepositories
{
    public interface IPhoneDetaildRepository
    {
        Task<PhoneDetaild> Add(PhoneDetaild obj);
        Task<PhoneDetaild> Update(PhoneDetaild obj);

        Task<List<PhoneDetaild>> GetAll();

        Task<PhoneDetaild> GetById(Guid id);
    }
}
