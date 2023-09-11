using AppData.Models;

namespace AppData.IRepositories
{
    public interface IChipGPURepository
    {
        Task<ChipGPUs> Add(ChipGPUs obj);
        Task<ChipGPUs> Update(ChipGPUs obj);
        Task<List<ChipGPUs>> GetAll();
        Task Delete(Guid id);

        Task<ChipGPUs> GetById(Guid id);
    }
}
