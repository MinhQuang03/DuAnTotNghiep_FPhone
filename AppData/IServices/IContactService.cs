using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Models;
using AppData.ViewModels.Options;

namespace AppData.IServices
{
    public interface IContactService
    {
        public Contact Add(Contact obj);
        public Contact Update(Contact obj);
        public List<Contact> ListContact(Contact SearchData,ListOptions listOptions);
        public Contact Details(Guid id);
    }
}
