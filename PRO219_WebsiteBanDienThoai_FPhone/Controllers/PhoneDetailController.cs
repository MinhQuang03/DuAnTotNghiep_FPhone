using AppData.IServices;
using AppData.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PRO219_WebsiteBanDienThoai_FPhone.Models;
using PRO219_WebsiteBanDienThoai_FPhone.ViewModel;

namespace PRO219_WebsiteBanDienThoai_FPhone.Controllers
{
    public class PhoneDetailController : Controller
    {
        private HttpClient _client;
        private IVwPhoneDetailService _phoneDetailService;
        private IListImageService _imageService;

        public PhoneDetailController(HttpClient client,IVwPhoneDetailService phoneDetailService,IListImageService ImageService)
        {
            _client = client;
            _phoneDetailService = phoneDetailService;
            _imageService = ImageService;
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
                lstImage = null
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
    }
}
