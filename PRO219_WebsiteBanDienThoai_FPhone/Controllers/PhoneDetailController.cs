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
using System.Drawing;

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
                Phone = await _phoneRepo.GetById(Guid.Parse(id)),
                IdAccount = User.Claims.FirstOrDefault(claim => claim.Type == "Id")?.Value
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
            data.ListReview = _phoneDetailService.GetListComment(id);
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
        public ActionResult CreateComment(string comment, string idAccount,string idPhone)
        {
            var isCreate = _phoneDetailService.CreateComment(comment, idAccount, idPhone);
            return Json(isCreate);
        }

     
    }
}
