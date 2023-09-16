using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AppData.IRepositories;
using AppData.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AppData.Repositories
{
    public class AccountStaffRepository : IAccountStaffRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AccountStaffRepository()
        {
            
        }

        public AccountStaffRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager;
        }

        public async Task<bool> SignUpAsync(SignUpModel model)
        {
            var user = new ApplicationUser
            {
                Name = model.FullName,
                Email = model.Email,
                Address = model.Address,
                CitizenId = model.CitizenId,
                PhoneNumber = model.PhoneNumber,
                Status = model.Status,
                ImageUrl = model.ImageUrl,
                PasswordHash = model.Password,
                UserName = model.UserName,
            };
            var result = await _userManager.CreateAsync(user, model.Password);

            // nếu khi tạo mới chưa có role là staff thì sẽ tạo mới 1 role là staff
            if (_roleManager.RoleExistsAsync("Staff") == null)
            {
                await _roleManager.CreateAsync(new IdentityRole("Staff"));
            }
            // add user với role là staff
            await _userManager.AddToRoleAsync(user, "Staff");
            if (result.Succeeded)
            {
                return true;
            }

            return false;
        }

        public async Task<string> SignInAsync(SignInModel model)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
            if (!result.Succeeded) return string.Empty;

            var authClaims = new List<Claim>
            {
                //new(ClaimTypes.Email, model.Email),
               
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AppSettings:SecretKey"]));

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("UserName", model.UserName),
                    new Claim("TokenId", Guid.NewGuid().ToString())
                }),
                Expires = DateTime.Now.AddHours(3),
                SigningCredentials = new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha512Signature)
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }

        public Task<string> GenerateToken(ApplicationUser model)
        {
            throw new NotImplementedException();
        }
    }
}
