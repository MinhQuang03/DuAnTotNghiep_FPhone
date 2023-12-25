using AppData.FPhoneDbContexts;
using AppData.IRepositories;
using AppData.IServices;
using AppData.Models;
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
        private FPhoneDbContext _context;
        public PhoneDetailController(HttpClient client,IVwPhoneDetailService phoneDetailService,IListImageService ImageService, IPhoneRepository phoneRepo)
        {
            _context = new FPhoneDbContext();
           _client = client;
            _phoneDetailService = phoneDetailService;
            _imageService = ImageService;
            _phoneRepo = phoneRepo;
        }
        public ActionResult PhoneDetail(string id)
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
                listImageByIdPhone = _imageService.GetListImageByIdPhone(Guid.Parse(id))
            };
            return View(data);
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
