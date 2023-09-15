using AppData.Models;

namespace AppData.IRepositories
{
    public interface IBatteryRepository
    {
        Task<Battery> Add(Battery obj);
        Task<Battery> Update(Battery obj);
        Task<List<Battery>> GetAll();
        Task Delete(Guid id);

        Task<Battery> GetById(Guid id);

    }
}
