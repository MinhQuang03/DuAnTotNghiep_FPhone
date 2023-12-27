using AppData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModels.Accounts
{
    public class ProfileVM
    {
        public Account User { get; set; }
        public Address Address { get; set; }
    }
}
