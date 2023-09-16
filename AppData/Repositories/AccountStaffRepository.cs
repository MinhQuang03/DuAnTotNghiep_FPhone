using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AppData.FPhoneDbContexts;
using AppData.IRepositories;
using AppData.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        private readonly FPhoneDbContext _dbContext;
        public AccountStaffRepository()
        {
            
        }

        public AccountStaffRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, RoleManager<IdentityRole> roleManager,FPhoneDbContext dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager; 
            _dbContext = dbContext;
        }

        public async Task<IdentityResult> SignUpAsync(SignUpModel model)
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
            if (result.Succeeded)
            {
                var us = await GetAllAsync();
                if (us.Count == 1) // nếu có 1 tài khoản ( trường hợp lần đầu tiên tạo tk )
                {
                    if (await _roleManager.RoleExistsAsync("Admin") == false)
                    {
                        await _roleManager.CreateAsync(new IdentityRole("Admin")); // tạo role admin nếu chưa có
                    }

                    await _userManager.AddToRoleAsync(user, "Admin"); // gán role admin cho user đầu tiên đó
                }
                else
                {
                    // nếu khi tạo mới chưa có role là staff thì sẽ tạo mới 1 role là staff
                    if (await _roleManager.RoleExistsAsync("Staff") == false)
                    {
                        await _roleManager.CreateAsync(new IdentityRole("Staff"));
                    }

                    // gán user với role là staff
                    await _userManager.AddToRoleAsync(user, "Staff");
                }
            }
            return result;
        }

        public async Task<string> SignInAsync(SignInModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
            if (!result.Succeeded) return string.Empty;
            var staff = _dbContext.AspNetUsers.FirstOrDefault(c => c.UserName == model.UserName);
            return await GenerateToken(staff);
        }

        public async Task<string> GenerateToken(ApplicationUser model)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            //var result = await _signInManager.PasswordSignInAsync(model.UserName, model.PasswordHash, false, false);
            //if (!result.Succeeded) return string.Empty; 
            var role = await _userManager.GetRolesAsync(model);
            var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AppSettings:SecretKey"]));

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("Id",model.Id),
                    new Claim("UserName",model.UserName),
                    new Claim(ClaimTypes.Role,string.Join(",",role)),
                    new Claim("TokenId", Guid.NewGuid().ToString())
                }),
                Expires = DateTime.Now.AddHours(3),
                SigningCredentials = new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha512Signature)
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }
        
        public async Task<List<ApplicationUser>> GetAllAsync()
        {
            return _dbContext.AspNetUsers.ToList();
        }
    }
}
