using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.ViewModels.Options;
using AppData.ViewModels.Phones;

namespace AppData.IServices
{
    public interface IVwPhoneService
    {
        List<VW_Phone_Group> listVwPhoneGroup(VW_Phone_Group model);
        List<VW_Phone_Group> listVwPhoneGroup(VW_Phone_Group model, ListOptions options);   
    }
}
