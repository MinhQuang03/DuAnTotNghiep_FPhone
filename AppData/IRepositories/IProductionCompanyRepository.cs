using AppData.Models;

namespace AppData.IRepositories
{
    public interface IProductionCompanyRepository
    {
        Task<ProductionCompany> Add(ProductionCompany obj);
        Task<ProductionCompany> Update(ProductionCompany obj);
        Task<List<ProductionCompany>> GetAll();
        Task Delete(Guid id);

        Task<ProductionCompany> GetById(Guid id);
    }
}
