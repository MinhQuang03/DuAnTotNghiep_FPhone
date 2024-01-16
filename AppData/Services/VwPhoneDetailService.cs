using AppData.FPhoneDbContexts;
using AppData.IServices;
using AppData.Models;
using AppData.Utilities;
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
                 (model.ProductionCompanyName == null || c.ProductionCompanyName.Contains(model.ProductionCompanyName))) &&
                (model.IdPhone == Guid.Empty || model.IdPhone == null || c.IdPhone.Equals(model.IdPhone))
            ).Skip(options.SkipCalc).Take(options.PageSize).ToList();

            options.AllRecordCount = _dbContext.VW_PhoneDetail.Count(c =>
                model == null ||
                ((model.Price == null || c.Price <= model.Price) &&
                 (model.PhoneName == null || c.PhoneName.Contains(model.PhoneName)) &&
                 (model.ChipCPUName == null || c.ChipCPUName.Contains(model.ChipCPUName)) &&
                 (model.MaterialName == null || c.MaterialName.Contains(model.MaterialName)) &&
                 (model.RamName == null || c.RamName.Contains(model.RamName)) &&
                 (model.RomName == null || c.RomName.Contains(model.RomName)) &&
                 (model.ProductionCompanyName == null || c.ProductionCompanyName.Contains(model.ProductionCompanyName))) &&
                (model.IdPhone == Guid.Empty || model.IdPhone == null || c.IdPhone.Equals(model.IdPhone))
            );
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return lst;
    }

    public List<VW_PhoneDetail> listVwPhoneDetails2(VW_PhoneDetail model, ListOptions options)
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
                 (model.ProductionCompanyName == null || c.ProductionCompanyName.Contains(model.ProductionCompanyName))) &&
                (model.IdPhone == Guid.Empty || model.IdPhone == null || c.IdPhone.Equals(model.IdPhone))
            ).Take(options.PageSize).ToList();

            options.AllRecordCount = _dbContext.VW_PhoneDetail.Count(c =>
                model == null ||
                ((model.Price == null || c.Price <= model.Price) &&
                 (model.PhoneName == null || c.PhoneName.Contains(model.PhoneName)) &&
                 (model.ChipCPUName == null || c.ChipCPUName.Contains(model.ChipCPUName)) &&
                 (model.MaterialName == null || c.MaterialName.Contains(model.MaterialName)) &&
                 (model.RamName == null || c.RamName.Contains(model.RamName)) &&
                 (model.RomName == null || c.RomName.Contains(model.RomName)) &&
                 (model.ProductionCompanyName == null || c.ProductionCompanyName.Contains(model.ProductionCompanyName))) &&
                (model.IdPhone == Guid.Empty || model.IdPhone == null || c.IdPhone.Equals(model.IdPhone))
            );
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return lst;
    }


    public List<PhoneDetaild> listPhoneDetailByIDPhone(Guid id)
    {
       var lst = new List<PhoneDetaild>();
       try
       {
            lst = _dbContext.PhoneDetailds.Where(c=>c.IdPhone == id).ToList();
       }
       catch (Exception e)
       {
           
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
        var lst = new VW_PhoneDetail();
        try
        {
             lst = _dbContext.VW_PhoneDetail.FirstOrDefault(c => c.IdPhoneDetail == id);
        }
        catch (Exception e)
        {
           
        }
        return lst;
    }
    public PhoneDetaild getPhoneDetailById(Guid id)
    {
        var lst = new PhoneDetaild();
        try
        {
            lst = _dbContext.PhoneDetailds.FirstOrDefault(c => c.Id == id);
        }
        catch (Exception e)
        {

        }
        return lst;
    }

    public int CheckPhoneDetail(Guid id)
    {
        return _dbContext.VW_PhoneDetail.Where(c => c.IdPhoneDetail == id).Count();
    }

    public int CountPhoneDetailFromImei(Guid idPhoneDetail)
    {
        return _dbContext.Imei.Count(c => c.IdPhoneDetaild == idPhoneDetail && c.Status == FphoneConst.ChuaBan);
    }

    public List<string> GetListImagebyIdPhoneDetail(Guid id)
    {
        return _dbContext.ListImage.Where(c => c.IdPhoneDetaild == id).Select(c => c.Image).ToList();
    }
    public PhoneDetaild Add(PhoneDetaild obj)
    {
         _dbContext.PhoneDetailds.AddAsync(obj);
         _dbContext.SaveChanges();
        return obj;
    }

    public PhoneDetaild Update(PhoneDetaild obj)
    {
        var dbo =  new PhoneDetaild();
        try
        {
            dbo = _dbContext.PhoneDetailds.FirstOrDefault(c => c.Id == obj.Id);
            BeanUtils.CopyAllPropertySameName(obj,dbo);
            _dbContext.PhoneDetailds.Update(dbo);
            _dbContext.SaveChanges();
        }
        catch (Exception e)
        {
           
        }
        
        return dbo;
    }
    public List<Review> GetListComment(string id)
    {
        return _dbContext.Reviews.Where(t => t.IdPhone.ToString() == id).ToList();
    }

    public int CreateComment(string comment, string idAccount, string idPhone)
    {
        Review review = new Review()
        {
            DateTime = DateTime.Now,
            Content = comment,
            IdAccount = Guid.Parse(idAccount),
            IdPhone = Guid.Parse(idPhone)
        };
        _dbContext.Reviews.Add(review);
        return _dbContext.SaveChanges();
    }
}