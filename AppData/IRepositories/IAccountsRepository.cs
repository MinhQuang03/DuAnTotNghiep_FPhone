using AppData.Models;
using AppData.ViewModels.Accounts;
using Microsoft.AspNetCore.Identity;

namespace AppData.IRepositories
{
    public interface IAccountsRepository
    {
        public Task<IdentityResult> SignUpAsync(SignUpModel model);
        public Task<LoginResponseVM> Login(LoginModel model);

        //protected Task<LoginResponseVM> GenerateToken(LoginInputVM model);
        public Task<List<ApplicationUser>> GetAllAsync();
    }
}
