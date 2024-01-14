using AppData.Models;
using AppData.ViewModels;
using AppData.ViewModels.Options;

namespace AppData.IServices
{
    public interface IAccountService
    {
        public List<ApplicationUser> GetAllAsync(ApplicationUser Search, ListOptions options);
        public ApplicationUser GetById(string id);
        public ApplicationUser Update(string id, ApplicationUser user,out DataError error);
        public Account UpdateUser(Guid id, Account user, out DataError error);
        public Account GetUserByEmail(string email);
        public Account GetUserById(Guid idGuid);
        public Account GetUserByPhoneNumber(string phoneNumber);
    }       
}
