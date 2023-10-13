using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Models;
using AppData.ViewModels.Roles;

namespace AppData.ViewModels.Accounts
{
    public class LoginResponseVM
    {
        public string? Name { get; set; } = "<Không có/Bị xóa>";
        public IList<string> Roles { get; set; }  = new List<string>();
        public bool Valid { get; set; } = false;
        public string? Token { get; set; }  
        public List<string> ListMessage { get; set; } = new List<string>();
        public List<string> ListErrorMessage { get; set; } = new List<string>();
    }
}
