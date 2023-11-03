using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Models;

namespace AppData.ViewModels.Accounts
{
    public class LoginInputVM
    {
        public ApplicationUser? ApplicationUser { get; set; } = new ApplicationUser();
        public Account? Account { get; set; } = new Account();
    }
}
