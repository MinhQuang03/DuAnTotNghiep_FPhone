using AppData.FPhoneDbContexts;
using AppData.IRepositories;
using AppData.IServices;
using Microsoft.AspNetCore.Mvc;
using PRO219_WebsiteBanDienThoai_FPhone.ViewModel;


namespace PRO219_WebsiteBanDienThoai_FPhone.Controllers
{
    public class ListPhoneController : Controller
    {
        private readonly HttpClient _client;
        private FPhoneDbContext _context;
        private IVwPhoneService _phoneService;
        private IProductionCompanyRepository _companyRepository;
        private IRamRepository _ramRepository;
        private IChipCPURepository _chipCpuRepository;
        private IRomRepository _romRepository;
        private IMaterialRepository _materialRepository;
        private IVwPhoneDetailService _phoneDetailService;

        public ListPhoneController(HttpClient client, FPhoneDbContext context, IVwPhoneService phoneService, IProductionCompanyRepository companyRepository, IRamRepository ramRepository, IChipCPURepository chipCpuRepository, IRomRepository romRepository, IMaterialRepository materialRepository, IVwPhoneDetailService phoneDetailService)
        {
            _client = client;
            _context = context;
            _phoneService = phoneService;
            _companyRepository = companyRepository;
            _ramRepository = ramRepository;
            _chipCpuRepository = chipCpuRepository;
            _romRepository = romRepository;
            _materialRepository = materialRepository;
            _phoneDetailService = phoneDetailService;
        }
        public async Task<IActionResult> Index()
        { 
            ListPhoneViewModel model = new ListPhoneViewModel();
            model.ListvVwPhoneDetails = _phoneDetailService.listVwPhoneDetails(model.SearchData, model.Options);
	       model.Brand = await _companyRepository.GetAll();
           model.listRam = await _ramRepository.GetAll();
           model.listChipCPU = await _chipCpuRepository.GetAll();
           model.listRom = await _romRepository.GetAll();
           model.listMaterial = await _materialRepository.GetAll();
	        return View(model);
		}

        [HttpPost]
        public async Task<IActionResult> Index(ListPhoneViewModel model)
        {
            model.ListvVwPhoneDetails = _phoneDetailService.listVwPhoneDetails(model.SearchData, model.Options);
            model.Brand = await _companyRepository.GetAll();
            model.listRam = await _ramRepository.GetAll();
            model.listChipCPU = await _chipCpuRepository.GetAll();
            model.listRom = await _romRepository.GetAll();
            model.listMaterial = await _materialRepository.GetAll();
            return View(model);
        }
       
        public async Task<IActionResult> OrderByDes()
        {
            ListPhoneViewModel model = new ListPhoneViewModel();
            model.ListvVwPhoneDetails = _phoneDetailService.listVwPhoneDetails(model.SearchData, model.Options).OrderByDescending(c =>c.Price).ToList();
            model.Brand = await _companyRepository.GetAll();
            model.listRam = await _ramRepository.GetAll();
            model.listChipCPU = await _chipCpuRepository.GetAll();
            model.listRom = await _romRepository.GetAll();
            model.listMaterial = await _materialRepository.GetAll();
            return View("Index", model);
        }

        public async Task<IActionResult> OrderByAsc()
        {
            ListPhoneViewModel model = new ListPhoneViewModel();
            model.ListvVwPhoneDetails = _phoneDetailService.listVwPhoneDetails(model.SearchData, model.Options)
                .OrderBy(c => c.Price).ToList();
            model.Brand = await _companyRepository.GetAll();
            model.listRam = await _ramRepository.GetAll();
            model.listChipCPU = await _chipCpuRepository.GetAll();
            model.listRom = await _romRepository.GetAll();
            model.listMaterial = await _materialRepository.GetAll();
            return View("Index", model);
        }

        public async Task<IActionResult> MorePhone()
        {
            ListPhoneViewModel model = new ListPhoneViewModel();
            model.Options.PageSize += 10;
            model.ListvVwPhoneDetails = _phoneDetailService.listVwPhoneDetails(model.SearchData, model.Options);
            model.Brand = await _companyRepository.GetAll();
            model.listRam = await _ramRepository.GetAll();
            model.listChipCPU = await _chipCpuRepository.GetAll();
            model.listRom = await _romRepository.GetAll();
            model.listMaterial = await _materialRepository.GetAll();
            return View("Index", model);
        }
    }
}
