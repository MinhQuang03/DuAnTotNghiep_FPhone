using AppData.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.IServices
{
    public interface IEmailService
    {
        public Task<ObjectEmailOutput> SendEmail(ObjectEmailInput obj);
    }
}
