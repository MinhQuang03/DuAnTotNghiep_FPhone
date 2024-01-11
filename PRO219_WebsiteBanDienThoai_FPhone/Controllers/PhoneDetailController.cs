using AppData.FPhoneDbContexts;
using AppData.IRepositories;
using AppData.IServices;
using AppData.Models;
using AppData.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PRO219_WebsiteBanDienThoai_FPhone.Models;
using PRO219_WebsiteBanDienThoai_FPhone.Services;
using PRO219_WebsiteBanDienThoai_FPhone.ViewModel;

namespace PRO219_WebsiteBanDienThoai_FPhone.Controllers
{
    public class PhoneDetailController : Controller
    {
        private HttpClient _client;
        private IVwPhoneDetailService _phoneDetailService;
        private IListImageService _imageService;
        private IPhoneRepository _phoneRepo;
        private IRamRepository _ramRepository;
        private FPhoneDbContext _context;
        public PhoneDetailController(HttpClient client,IVwPhoneDetailService phoneDetailService,IListImageService ImageService, IPhoneRepository phoneRepo,IRamRepository ramRepository)
        {
            _context = new FPhoneDbContext();
           _client = client;
            _phoneDetailService = phoneDetailService;
            _imageService = ImageService;
            _phoneRepo = phoneRepo;
            _ramRepository = ramRepository;
        }
        public async Task<ActionResult> PhoneDetail(string id)
        {
      
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }
            var data = new VwProductDetailViewModel()
            {
                Records = _phoneDetailService.getListPhoneDetailByIdPhone(Guid.Parse(id)),
                lstImage = null,
                Image = _phoneRepo.GetById(Guid.Parse(id)).Result.Image,
                listImageByIdPhone = _imageService.GetListImageByIdPhone(Guid.Parse(id)),
                IdPhone = id,
                Phone = await _phoneRepo.GetById(Guid.Parse(id))
            };

            var phoneDetail = _phoneDetailService.listPhoneDetailByIDPhone(Guid.Parse(id));
            if (phoneDetail!=null)
            {
                var lstIdRam = phoneDetail.GroupBy(c => c.IdRam).Select(c => c.Key).ToList();
                if (lstIdRam!=null)
                {
                    foreach (var item in lstIdRam)
                    {
                        data.LstRam.Add( await _ramRepository.GetById(item));
                    }
                }
            }
            return View(data);
        }
        [HttpGet("/PhoneDetail/getListPhoneDetailByIdPhone/{id}/{idRam}")]
        public JsonResult getListPhoneDetailByIdPhone(string id,string idRam)
        {
            return Json(_phoneDetailService.getListPhoneDetailByIdPhone(Guid.Parse(id)).Where(c =>c.RamID == Guid.Parse(idRam)).ToList());
        }

        [HttpGet("/PhoneDetail/getPhoneDetailById/{id}")]
        public JsonResult getPhoneDetailById(string id)
        {
            return Json(_phoneDetailService.getPhoneDetailByIdPhoneDetail(Guid.Parse(id)));
        }
        [HttpGet("/PhoneDetail/checkExitImei/{idPhoneDetail}")]
        public JsonResult checkExitImei(string idPhoneDetail)
        {
            var data = _context.Imei
                .Where(c => c.IdPhoneDetaild == Guid.Parse(idPhoneDetail) && c.Status == FphoneConst.ChuaBan).ToList();
            if (data!=null)
            {
                return Json(data.Count);
            }
            return Json(null);
        }

        [HttpGet]
        public ActionResult GetDetailPhones(string id)
        {
            var data = new VwProductDetailViewModel();
            data.Records = _phoneDetailService.getListPhoneDetailByIdPhone(Guid.Parse(id));

            return Json(new
            {
                Items = data.Records,
                Success = true
            });
        }

        [HttpPost]
        public void AddReview(Guid idAccount, Guid idPhoneDetaild, string content)
        {
            // Create a new Review instance
            var newReview = new Review
            {
                Id = Guid.NewGuid(), // Generate a new GUID for the review
                DateTime = DateTime.Now,
                Content = content,
                IdPhoneDetaild = idPhoneDetaild,
                IdAccount = idAccount
            };

            // Add the new review to the database context
            _context.Reviews.Add(newReview);

            // Save changes to the database
            _context.SaveChanges();
        }
    }
}
