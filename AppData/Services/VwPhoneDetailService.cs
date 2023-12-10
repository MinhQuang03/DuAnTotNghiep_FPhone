using AppData.FPhoneDbContexts;
using AppData.IServices;
using AppData.ViewModels.Options;
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

    public List<VW_PhoneDetail> listVwPhoneDetails(VW_PhoneDetail model)
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

    public List<VW_PhoneDetail> listVwPhoneDetails(VW_PhoneDetail model, ListOptions options)
    {
        var lst = new List<VW_PhoneDetail>();
        var countRecords = _dbContext.VW_PhoneDetail.Count();
        //nếu pagesize lớn hơn số lượng bản ghi thì lấy ra rất cả bản ghi
        //if (options.PageSize >= countRecords) options.PageSize = countRecords;
        try
        {
            lst = _dbContext.VW_PhoneDetail.Where(c =>
                model == null ||
                ((model.Price == null || c.Price <= model.Price) &&
                 (model.PhoneName == null || c.PhoneName.Contains(model.PhoneName)) &&
                 (model.ChipCPUName == null || c.ChipCPUName.Contains(model.ChipCPUName)) &&
                 (model.MaterialName == null || c.MaterialName.Contains(model.MaterialName)) &&
                 (model.RamName == null || c.RamName.Contains(model.RamName)) &&
                 (model.RomName == null || c.RomName.Contains(model.RomName)) &&
                 (model.ProductionCompanyName == null || c.ProductionCompanyName.Contains(model.ProductionCompanyName)))
            ).Skip(options.SkipCalc).Take(options.PageSize).ToList();

            options.AllRecordCount = _dbContext.VW_PhoneDetail.Count(c => model == null ||
                                                                          ((model.Price == null ||
                                                                            c.Price <= model.Price) &&
                                                                           (model.PhoneName == null ||
                                                                            c.PhoneName.Contains(model.PhoneName)) &&
                                                                           (model.ChipCPUName == null ||
                                                                            c.ChipCPUName.Contains(
                                                                                model.ChipCPUName)) &&
                                                                           (model.MaterialName == null ||
                                                                            c.MaterialName.Contains(
                                                                                model.MaterialName)) &&
                                                                           (model.RamName == null ||
                                                                            c.RamName.Contains(model.RamName)) &&
                                                                           (model.RomName == null ||
                                                                            c.RomName.Contains(model.RomName)) &&
                                                                           (model.ProductionCompanyName == null ||
                                                                            c.ProductionCompanyName.Contains(
                                                                                model.ProductionCompanyName))));
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