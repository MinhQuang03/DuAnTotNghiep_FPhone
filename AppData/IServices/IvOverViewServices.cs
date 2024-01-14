using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.ViewModels.ThongKe;
using AppData.Models;
using AppData.ViewModels.Options;
using AppData.ViewModels.Phones;

namespace AppData.IServices
{
    public interface IvOverViewServices
    {
      List<vOverView> listOverViewGroup();
    }
}
