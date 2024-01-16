using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AppData.FPhoneDbContexts;
using AppData.IRepositories;
using AppData.Models;
using AppData.Utilities;
using AppData.ViewModels.Accounts;
using Microsoft.AspNetCore.Identity;
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


        public async Task<IdentityResult> UpdateRole(string id,string role)
        {
          var model =  _dbContext.AspNetUsers.FirstOrDefault(c => c.Id == id);
          IdentityResult result = new IdentityResult();
          if (model != null)
          {
              if (await _roleManager.RoleExistsAsync(role) == false)
              {
                  await _roleManager.CreateAsync(new IdentityRole(role)); // tạo role nếu chưa có
              }
              result = await _userManager.AddToRoleAsync(model, role);
          }
          return result;
        }


        public async Task<bool> SignUpCl(ClAccountsViewModel model)
        {
            Security security = new Security();
            
            try
            {
                Account ac = new Account()
                {
                    Id = model.Id,
                    Email = model.Email,
                    ImageUrl = model.ImageUrl,
                    Name = model.Name,
                    Password = security.Encrypt("B3C1035D5744220E", model.Password),
                    Username = model.Username,
                    PhoneNumber = model.PhoneNumber,
                    Points = model.Points,
                    Status = model.Status,
                };
                if (!_dbContext.AspNetUsers.Any(c => c.UserName == model.Username) && !_dbContext.Accounts.Any(c =>c.Username == model.Username))
                {
               
                        await _dbContext.Accounts.AddAsync(ac);
                        await _dbContext.SaveChangesAsync();
                   
                    //ObjectEmailInput email = new ObjectEmailInput();
                    ////Gửi email
                    //email.UserName = model.Username;
                    //email.FullName = model.Name;
                    //email.Subject = "Thông báo tạo tài khoản thành công";
                    //email.Message = Utility.EmailCreateAccountTemplate;
                    //email.SendTo = model.Email;
                    //await Utility.SendEmail(email);
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

        public async Task<LoginResponseVM> AdLogin(LoginModel model)
        {
            LoginInputVM x = new LoginInputVM();
            var adminResult = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
            if (adminResult.Succeeded)
            {
                var staff = _dbContext.AspNetUsers.FirstOrDefault(c => c.UserName == model.UserName);
                if (staff.Status ==FphoneConst.HoatDong)
                {
                    x.ApplicationUser = staff;
                    return await GenerateToken(x);
                }
            }
            return await GenerateToken(x);
        }

        public async Task<LoginResponseVM> ClLogin(LoginModel model)
        {
            LoginInputVM x = new LoginInputVM();
            Security security = new Security();
            var userResult = _dbContext.Accounts.FirstOrDefault(c => c.Username == model.UserName);
            if (userResult != null && userResult.Status == FphoneConst.HoatDong)
            {
                string giaima = security.Encrypt("B3C1035D5744220E", model.Password);
                if (userResult.Password == giaima)
                {
                    x.Account = userResult;
                    return await GenerateToken(x);
                }
                
            }
            return await GenerateToken(x);
        }

        //public async Task<LoginResponseVM> Login(LoginModel model)
        //{
        //    LoginInputVM x = new LoginInputVM();
        //    var adminResult = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
        //    var userResult = _dbContext.Accounts.AsNoTracking().FirstOrDefault(c => c.Username == model.UserName && c.Password == model.Password);
           
        //    if (adminResult.Succeeded)
        //    {
        //        var staff = _dbContext.AspNetUsers.FirstOrDefault(c => c.UserName == model.UserName);
        //        x.ApplicationUser = staff;
        //        return await GenerateToken(x);
        //    }

        //    if (userResult != null)
        //    {
        //        x.Account = userResult;
        //        return await GenerateToken(x);
        //    }

        //    return await GenerateToken(x);
        //}

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

            if (model is { ApplicationUser.UserName: not null, Account.Username: null })
            {
                var role = await _userManager.GetRolesAsync(model.ApplicationUser);
                var tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("Id",model.ApplicationUser.Id),
                        new Claim(ClaimTypes.Name,model.ApplicationUser.Name),
                        new Claim(ClaimTypes.Role,role.FirstOrDefault() ?? string.Empty),
                        
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

            if (model is { Account.Username: not null, ApplicationUser.UserName: null })
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
