using AppData.Models;

namespace AppData.IRepositories
{
    public interface IColorRepository
    {
        Task<Color?> Add(Color? obj);
        Task<Color> Update(Color obj);
        Task<List<Color?>> GetAll();
        Task Delete(Guid id);

        Task<Color?> GetById(Guid id);
    }
}
