using AppData.FPhoneDbContexts;
using AppData.IRepositories;
using AppData.Models;
using Microsoft.EntityFrameworkCore;

namespace AppData.Repositories
{
    public class ImeiRepository : IImeiRepository
    {
        public readonly FPhoneDbContext _dbContext;
        public ImeiRepository(FPhoneDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Imei> Add(Imei obj)
        {
            // Kiểm tra xem có Imei nào có cùng NameImei chưa
            var existingImei = await _dbContext.Imei.FirstOrDefaultAsync(i => i.NameImei == obj.NameImei);

            if (existingImei != null)
            {
                // Nếu đã tồn tại Imei với cùng NameImei, bạn có thể xử lý tùy thuộc vào yêu cầu của bạn,
                // ví dụ: throw exception, trả về null, hoặc thực hiện các bước khác.
                throw new Exception("Imei with the same NameImei already exists");
            }

            await _dbContext.Imei.AddAsync(obj);
            await _dbContext.SaveChangesAsync();
            return obj;
        }

        public async Task Delete(Guid id)
        {
            var a = await _dbContext.Imei.FindAsync(id);
            _dbContext.Imei.Remove(a);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Imei>> GetAll()
        {
            return await _dbContext.Imei.ToListAsync();
        }

        public async Task<Imei> GetById(Guid id)
        {
            return await _dbContext.Imei.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Imei> Update(Imei obj)
        {
            var a = await _dbContext.Imei.FindAsync(obj.Id);
            a.NameImei = obj.NameImei;
            a.Status = obj.Status;
            a.IdPhoneDetaild = obj.IdPhoneDetaild;
            _dbContext.Imei.Update(a);
            await _dbContext.SaveChangesAsync();
            return obj;
        }
    }
}
