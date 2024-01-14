using AppData.Models;
using AppData.ViewModels.Accounts;
using Microsoft.AspNetCore.Identity;

namespace AppData.IRepositories
{
    public interface IAccountsRepository
    {
        public Task<IdentityResult> SignUpAdmin(AdSignUpViewModel model);
        public Task<IdentityResult> UpdateRole(string id, string role);
        public Task<bool> SignUpCl(ClAccountsViewModel model);  
        public Task<LoginResponseVM> AdLogin(LoginModel model);  
        public Task<LoginResponseVM> ClLogin(LoginModel model); 

        //protected Task<LoginResponseVM> GenerateToken(LoginInputVM model);
        public Task<List<ApplicationUser>> GetAllAsync();
      
    }
}
