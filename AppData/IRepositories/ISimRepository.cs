using AppData.Models;

namespace AppData.IRepositories
{
    public interface ISimRepository
    {
        Task<Sim> Add(Sim obj);
        Task<Sim> Update(Sim obj);
        Task<List<Sim>> GetAll();
        Task Delete(Guid id);

        Task<Sim> GetById(Guid id);
    }
}
