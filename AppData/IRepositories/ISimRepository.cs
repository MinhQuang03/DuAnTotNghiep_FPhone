using AppData.Models;

namespace AppData.IRepositories
{
    public interface ISimRepository
    {
        Task<bool> Create(Sim obj);
    }
}
