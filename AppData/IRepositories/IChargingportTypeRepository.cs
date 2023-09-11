using AppData.Models;

namespace AppData.IRepositories
{
    public interface IChargingportTypeRepository
    {
        Task<ChargingportType> Add(ChargingportType obj);
        Task<ChargingportType> Update(ChargingportType obj);
        Task<List<ChargingportType>> GetAll();
        Task Delete(Guid id);

        Task<ChargingportType> GetById(Guid id);
    }
}
