using AppData.IServices;
using AppData.Models;
using AppData.Utilities;
using AppData.ViewModels;
using Microsoft.AspNetCore.Mvc;
using PRO219_WebsiteBanDienThoai_FPhone.ViewModel;

namespace PRO219_WebsiteBanDienThoai_FPhone.Controllers
{
    public class ContactController : Controller
    {
        private IContactService _service;

        public ContactController(IContactService service)
        {
            _service = service;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(ContactViewModel obj)
        {
            var data = new Contact();
            data.CODE = Utility.RandomString(8);
            data.FullName = obj.FullName;
            data.PhoneNumber = obj.PhoneNumber;
            data.Email = obj.Email;
            data.Address = obj.Address;
            data.Status = 0;
            data.Topic = obj.Topic;
            data.Content = obj.Content;
            data.CreateDate = DateTime.Now;
            data.Type = "LIEN_HE";
          var dbo=  _service.Add(data);
          if (dbo==null)
          {
              return View("DataError");
          }
        
          return View("Success");
        }
    }
}
