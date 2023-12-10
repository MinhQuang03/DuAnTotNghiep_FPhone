using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using AppData.IRepositories;
using AppData.Repositories;
using AppData.FPhoneDbContexts;

namespace AppData.Ultilities
{
    public class Validation
    {
        private FPhoneDbContext _dbContexts;
        public IUserRepository _user;
        public Validation(FPhoneDbContext dbContexts)
        {
            _dbContexts = dbContexts;
            _user = new UserRepostitory(dbContexts);
        }
        public bool CheckString(string s)
        {
            var regexItem = new Regex("^[a-zA-Z0-9 ]*$");
            return regexItem.IsMatch(s);
        }

        public bool isNumeric(string str)
        {
            return int.TryParse(str, out int n);
        }
        
    
    }
}
