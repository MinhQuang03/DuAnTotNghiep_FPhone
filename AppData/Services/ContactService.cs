using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.FPhoneDbContexts;
using AppData.IServices;
using AppData.Models;

namespace AppData.Services
{
    public class ContactService:IContactService
    {
        private FPhoneDbContext _context;

        public ContactService(FPhoneDbContext context)
        {
            _context = context;
        }
        public Contact Add(Contact obj)
        {
            try
            {
                _context.Contact.Add(obj);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
               
            }

            return obj;
        }

        public Contact Update(Contact obj)
        {
            var data = new Contact();
            try
            {
                data = _context.Contact.FirstOrDefault(c => c.ID == obj.ID);
                data.FullName = obj.FullName;
                data.PhoneNumber = obj.PhoneNumber;
                data.Email = obj.Email;
                data.Address = obj.Address;
                data.Status = obj.Status;
                data.Topic = obj.Topic;
                data.Content = obj.Content;
                data.Type = obj.Type;
                _context.Contact.Update(data);
                _context.SaveChanges();
            }
            catch (Exception e)
            {

            }

            return obj;
        }
    }
}
