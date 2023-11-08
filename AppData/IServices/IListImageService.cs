using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Models;

namespace AppData.IServices
{
    public interface IListImageService
    {
        List<ListImage> GetListImagesByIdPhoneDetail(Guid IdPhoneDetail);
    }   
}
