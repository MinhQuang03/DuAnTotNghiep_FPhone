using AppData.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.FPhoneDbContexts;
using AppData.Models;

namespace AppData.Services
{
    public class CartDetailService: ICartDetailService
    {
        private FPhoneDbContext _dbContext;

        public CartDetailService(FPhoneDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<CartDetails> GetCartDetailsByIdAccount(Guid id)
        {
            var lst = new List<CartDetails>();
            try
            {
               lst = _dbContext.CartDetails.Where(c => c.IdAccount == id).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return lst;
        }
    }
}
