using AppData.Models;
using Microsoft.AspNetCore.Identity;

namespace AppData.IRepositories
{
    public interface IAccountStaffRepository
    {
        public Task<IdentityResult> SignUpAsync(SignUpModel model);
        public Task<string> SignInAsync(SignInModel model);

        public Task<string> GenerateToken(ApplicationUser model);
        public Task<List<ApplicationUser>> GetAllAsync();
    }
}
