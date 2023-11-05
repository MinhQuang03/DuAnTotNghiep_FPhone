using AppData.FPhoneDbContexts;
using AppData.IRepositories;
using AppData.IServices;
using AppData.Models;
using AppData.ViewModels.Phones;
using Microsoft.EntityFrameworkCore;

namespace AppData.Services
{
    public class PhoneDetailService : IPhoneDetailService
    {
        private IPhoneDetaildRepository _iPhoneDetaildRepository;
        private IPhoneRepository _iPhoneRepository;
        private IColorRepository _colorRepository;
        private IProductionCompanyRepository _productionCompanyRepository;
        private IRamRepository _ramRepository;
        private IRomRepository _romRepository;
       
        public readonly FPhoneDbContext _dbContext;
        public PhoneDetailService(IPhoneDetaildRepository iPhoneDetaildRepository, IPhoneRepository phoneRepository,FPhoneDbContext dbContext,IColorRepository colorRepository,IProductionCompanyRepository productionCompanyRepository,IRamRepository ramRepository, IRomRepository romRepository)    
        {
            _iPhoneDetaildRepository = iPhoneDetaildRepository;
            _iPhoneRepository = phoneRepository;
            _dbContext = dbContext;
            _colorRepository = colorRepository;
            _productionCompanyRepository = productionCompanyRepository;
            _ramRepository = ramRepository;
            _romRepository = romRepository;
        }

        public async Task<PhoneDetaild> Add(PhoneDetaild obj)
        {
            await _dbContext.PhoneDetailds.AddAsync(obj);
            await _dbContext.SaveChangesAsync();
            return obj;
        }

        public async Task<PhoneDetaild> GetById(Guid id)
        {
            return await _dbContext.PhoneDetailds.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<PhoneDetaild>> GetPhoneDetailds()
        {
            var results = await _iPhoneDetaildRepository.GetAll();
            foreach (var phoneDetaild in results)
            {
                phoneDetaild.Phones = await _iPhoneRepository.GetById(phoneDetaild.IdPhone);
                // sau cần lấy thông tin khác thì add dependency repository vào rồi get tương tự
            }
            return results;
        }
        public async Task<List<PhoneDetaild>> GetPhoneDetailds(Guid IdPhone)
        {
            var results = await _iPhoneDetaildRepository.GetAll(IdPhone);
            foreach (var phoneDetaild in results)
            {
                phoneDetaild.Phones = await _iPhoneRepository.GetById(phoneDetaild.IdPhone);
                phoneDetaild.Colors = await _colorRepository.GetById(phoneDetaild.IdColor);
                phoneDetaild.Phones.ProductionCompanies =
                    await _productionCompanyRepository.GetById(phoneDetaild.Phones.IdProductionCompany);
                phoneDetaild.Rams = await _ramRepository.GetById(phoneDetaild.IdRam);
                phoneDetaild.Roms = await _romRepository.GetById(phoneDetaild.IdRom);
            }
            return results;
        }

        public async Task<PhoneDetaild> Update(PhoneDetaild obj)
        {
            var a = await _dbContext.PhoneDetailds.FindAsync(obj.Id);
            a.IdPhone = obj.IdPhone;
            a.IdDiscount = obj.IdDiscount;
            a.IdMaterial = obj.IdMaterial;
            a.IdRom = obj.IdRom;
            a.IdRam = obj.IdRam;
            a.IdOperatingSystem = obj.IdOperatingSystem;
            a.IdBattery = obj.IdBattery;
            a.IdSim = obj.IdSim;
            a.IdChipCPU = obj.IdChipCPU;
            a.IdChipGPU = obj.IdChipGPU;
            a.IdColor = obj.IdColor;
            a.IdChargingport = obj.IdChargingport;
            a.Weight = obj.Weight;
            a.FrontCamera = obj.FrontCamera;
            a.MainCamera = obj.MainCamera;
            a.Resolution = obj.Resolution;
            a.Size = obj.Size;
            a.Price = obj.Price;
            a.Status = obj.Status;
            _dbContext.PhoneDetailds.Update(a);
            await _dbContext.SaveChangesAsync();
            return obj;
        }

        public Task<PhoneDetaild> GetPhoneByColor(Guid IdColor)
        {
            return _iPhoneDetaildRepository.GetPhoneByColor(IdColor);
        }

        public Task<PhoneDetaild> GetPhoneByRam(Guid IdRam)
        {
            return _iPhoneDetaildRepository.GetPhoneByRam(IdRam);
        }

        public Task<PhoneDetaild> GetPhoneByChipCPUs(Guid IdChipCPUs)
        {
            return _iPhoneDetaildRepository.GetPhoneByChipCPUs(IdChipCPUs);
        }

        public Task<List<PhoneDetaild>> GetPhoneDESC()
        {
            return _iPhoneDetaildRepository.GetPhoneDESC();
        }

        public Task<List<PhoneDetaild>> GetPhoneASC()
        {
            return _iPhoneDetaildRepository.GetPhoneASC();
        }

        public Task<List<PhoneDetaild>> GetPhone5tr()
        {
            return _iPhoneDetaildRepository.GetPhone5tr();
        }

        public Task<List<PhoneDetaild>> GetPhone10tr()
        {
            return _iPhoneDetaildRepository.GetPhone10tr();
        }

        public Task<List<PhoneDetaild>> GetPhone15tr()
        {
            return _iPhoneDetaildRepository.GetPhone15tr();
        }

        public Task<List<PhoneDetaild>> GetPhone25tr()
        {
            return _iPhoneDetaildRepository.GetPhone25tr();
        }

        public Task<List<PhoneDetaild>> GetPhone50tr()
        {
            return _iPhoneDetaildRepository.GetPhone50tr();
        }

       
    }
}
