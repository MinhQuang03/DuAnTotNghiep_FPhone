using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.FPhoneDbContexts;
using AppData.IRepositories;
using AppData.Models;
using Microsoft.EntityFrameworkCore;

namespace AppData.Repositories
{
    public class AddressRepostitory :IAddressRepository
    {
        private FPhoneDbContext _dbContext;

        public AddressRepostitory(FPhoneDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Task<Address?> GetAddress(Guid IdUser)
        {
            return _dbContext.Address.AsNoTracking().FirstOrDefaultAsync(c => c.IdAccount == IdUser && c.Status == 1);
        }

        public async Task<Address> UpdateAddress(Address address)
        {
            var result = _dbContext.Address.FirstOrDefault(c=>c.Id == address.Id && c.IdAccount == address.IdAccount);
            if (result != null)
            {
                result.Country = address.Country;
                result.City = address.City;
                result.District = address.District;
                result.HomeAddress = address.HomeAddress;
                result.Status = address.Status;
                await _dbContext.SaveChangesAsync(); // Use asynchronous SaveChanges

                return result;
            }
            return null;
        }
    }
}
