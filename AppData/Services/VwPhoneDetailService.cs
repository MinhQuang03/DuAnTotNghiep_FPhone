using AppData.FPhoneDbContexts;
using AppData.IServices;
using AppData.ViewModels.Phones;
using Microsoft.EntityFrameworkCore;

namespace AppData.Services;

public class VwPhoneDetailService : IVwPhoneDetailService
{
    private readonly FPhoneDbContext _dbContext;

    public VwPhoneDetailService(FPhoneDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public List<VW_PhoneDetail> listVwPhoneDetails()
    {
        var lst = new List<VW_PhoneDetail>();
        try
        {
            lst = _dbContext.VW_PhoneDetail.ToList();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return lst;
    }

    public List<VW_PhoneDetail> getListPhoneDetailByIdPhone(Guid idPhone)
    {
        var lst = new List<VW_PhoneDetail>();
        try
        {
            lst = _dbContext.VW_PhoneDetail.AsNoTracking().Where(c => c.IdPhone == idPhone).ToList();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return lst;
    }

    public VW_PhoneDetail getPhoneDetailByIdPhoneDetail(Guid id)
    {
       var lst = _dbContext.VW_PhoneDetail.FirstOrDefault(c => c.IdPhoneDetail == id);
       return lst;
    }

    public int CheckPhoneDetail(Guid id)
    {
        return _dbContext.VW_PhoneDetail.Where(c => c.IdPhoneDetail == id).Count();
    }
}