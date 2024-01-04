
using System.Collections;
using AppData.IServices;
using AppData.Models;
using AppData.Utilities;
using Microsoft.AspNetCore.Mvc;
using PRO219_WebsiteBanDienThoai_FPhone.ViewModel;

namespace PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdContactController : Controller
    {
        private IContactService _service;

        public AdContactController(IContactService service)
        {
            _service = service;
        }
        public IActionResult Index()
        {
            ContactViewModel obj = new ContactViewModel();
            List<Contact> lst = new List<Contact>();
            obj.SearchData.Status = 0;
            lst = _service.ListContact(obj.SearchData, obj.ListOptions);

            for (int i = 0; i < lst.Count; i++)
            {
                var item = new Contact();
                BeanUtils.CopyAllPropertySameName(lst[i], item);
                obj.Records.Add(item);
            }
            return View(obj);
        }

        public IActionResult ChangeStatus(Guid id)
        {
            var contact = new Contact();
            var data = _service.Details(id);
            if (data!=null)
            {
                data.Status = 1;
                data.ModifyDate = DateTime.Now;
                BeanUtils.CopyAllPropertySameName(data,contact);
                _service.Update(contact);
            }
            return RedirectToAction("Index");
        }

        public IActionResult ContactHis(Guid id)
        {
            ContactViewModel obj = new ContactViewModel();
            List<Contact> lst = new List<Contact>();
            obj.SearchData.Status = 1;
            lst = _service.ListContact(obj.SearchData, obj.ListOptions);

            for (int i = 0; i < lst.Count; i++)
            {
                var item = new Contact();
                BeanUtils.CopyAllPropertySameName(lst[i], item);
                obj.Records.Add(item);
            }
            return View(obj);
        }    

    }
}
