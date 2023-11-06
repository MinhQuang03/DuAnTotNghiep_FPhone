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
        public async Task<IActionResult> PhoneDetail(Guid IdPhone)
        {
            var data = new VwProductDetailViewModel();
            data.Records = _phoneDetailService.getListPhoneDetailByIdPhone(IdPhone);
            return View(data);
        }
    }
}
