using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AppData.FPhoneDbContexts;
using AppData.IRepositories;
using AppData.Models;
using AppData.ViewModels.Accounts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AppData.Repositories
{
    public class AccountsRepository : IAccountsRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private FPhoneDbContext _dbContext;

        public AccountsRepository()
        {

        }

        public AccountsRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, RoleManager<IdentityRole> roleManager,FPhoneDbContext dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager;
            _dbContext = dbContext;
        }

        public async Task<IdentityResult> SignUpAdmin(AdSignUpViewModel model)
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
                if (await _roleManager.RoleExistsAsync(model.Role) == false)
                {
                    await _roleManager.CreateAsync(new IdentityRole(model.Role)); // tạo role nếu chưa có
                }
                await _userManager.AddToRoleAsync(user, model.Role);
            }
            return result;
        }

        public async Task<bool> SignUpCl(ClAccountsViewModel model)
        {
            try
            {
                Account ac = new Account()
                {
                    Id = model.Id,
                    Email = model.Email,
                    ImageUrl = model.ImageUrl,
                    Name = model.Name,
                    Password = model.Password,
                    Username = model.Username,
                    PhoneNumber = model.PhoneNumber,
                    Points = model.Points,
                    Status = model.Status,
                };
                if (!_dbContext.AspNetUsers.Any(c => c.UserName == model.Username) && !_dbContext.Accounts.Any(c =>c.Username == model.Username))
                {
                    await _dbContext.Accounts.AddAsync(ac);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                return false;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<LoginResponseVM> Login(LoginModel model)
        {
            LoginInputVM x = new LoginInputVM();
            var adminResult = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
            var userResult = _dbContext.Accounts.AsNoTracking()
                .FirstOrDefault(c => c.Username == model.UserName && c.Password == model.Password);
           
            if (adminResult.Succeeded)
            {
                var staff = _dbContext.AspNetUsers.FirstOrDefault(c => c.UserName == model.UserName);
                x.ApplicationUser = staff;
                return await GenerateToken(x);
            }

            if (userResult != null)
            {
                x.Account = userResult;
                return await GenerateToken(x);
            }

            return await GenerateToken(x);
        }

        private async Task<LoginResponseVM> GenerateToken(LoginInputVM model)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AppSettings:SecretKey"]));
            var result = new LoginResponseVM();
            if (model == null)
            {
                result.ListErrorMessage.Add("Tài khoản hoặc mật khẩu không đúng");
                result.Valid = false;   
                return result;
            }

            if (model is { ApplicationUser: not null, Account: null })
            {
                var role = await _userManager.GetRolesAsync(model.ApplicationUser);
                var tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("Id",model.ApplicationUser.Id),
                        new Claim(ClaimTypes.Name,model.ApplicationUser.Name),
                        new Claim(ClaimTypes.Role,string.Join(",",role)),
                    }),
                    Expires = DateTime.Now.AddHours(3),
                    SigningCredentials = new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha512Signature)
                };
                 var token = jwtTokenHandler.CreateToken(tokenDescriptor);
                 result.Name = model.ApplicationUser.Name;
                 result.Valid = true;
                result.Roles = role;
                 result.ListMessage.Add("Đã đăng nhập với quyền admin");
                 result.Token = jwtTokenHandler.WriteToken(token);
                 return result;
            }

            if (model is { Account: not null, ApplicationUser: null })
            {
                var tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("Id",model.Account.Id.ToString()),
                        new Claim(ClaimTypes.Name,model.Account.Name),
                        new Claim(ClaimTypes.Role,"User"),
                    }),
                    Expires = DateTime.Now.AddHours(3),
                    SigningCredentials = new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha512Signature)
                };
                var token = jwtTokenHandler.CreateToken(tokenDescriptor);
                result.Name = model.Account.Name;
                result.Valid = true;
                result.Roles.Add("User");
                result.ListMessage.Add("Đã đăng nhập với tư cách người dùng");
                result.Token = jwtTokenHandler.WriteToken(token);
                return result;
            }

            return result;
        }
        
        public async Task<List<ApplicationUser>> GetAllAsync()
        {
            return _dbContext.AspNetUsers.ToList();
        }
    }
}
