using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.FPhoneDbContexts;
using AppData.IServices;
using AppData.Models;
using AppData.Utilities;
using AppData.ViewModels;
using AppData.ViewModels.Options;
using Microsoft.AspNetCore.Identity;

namespace AppData.Services
{
    public class AccountService :IAccountService
    {
        private FPhoneDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        public AccountService(FPhoneDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public  List<ApplicationUser> GetAllAsync(ApplicationUser Search, ListOptions options)
        {
            var lst = new List<ApplicationUser>();
            try
            {
                lst = _dbContext.AspNetUsers.Where(c => Search == null ||
                                                        (Search.Name == null || c.Name.Contains(Search.Name)) &&
                                                        (Search.PhoneNumber == null || c.PhoneNumber.Contains(Search.PhoneNumber))&&(Search.UserName == null|| c.UserName.Contains(Search.UserName))
                ).Skip(options.SkipCalc).Take(options.PageSize).ToList();
                options.AllRecordCount = lst.Count(c => Search == null ||
                                                        (Search.Name == null || c.Name.Contains(Search.Name)) &&
                                                        (Search.PhoneNumber == null ||
                                                         c.PhoneNumber.Contains(Search.PhoneNumber)) &&
                                                        (Search.UserName == null ||
                                                         c.UserName.Contains(Search.UserName)));
            }
            catch (Exception e)
            {
               
            }

            return lst;
        }

        public ApplicationUser GetById(string id)
        {
            var user = new ApplicationUser();
            try
            {
                user = _dbContext.AspNetUsers.Find(id);
                user.Role = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
            }
            catch (Exception e)
            {
               
            }

            return user;
        }

        public ApplicationUser GetByUserName(string userName)
        {
            var user = new ApplicationUser();
            try
            {
                user = _dbContext.AspNetUsers.FirstOrDefault( c=>c.UserName == userName);
            }
            catch (Exception e)
            {

            }

            return user;
        }

        public ApplicationUser Update(string id, ApplicationUser user,out DataError error)
        {
            error = new DataError() { Success = true };
            var dbo = _dbContext.AspNetUsers.Find(id);
            try
            {
                dbo.Status = user.Status;
                dbo.UserName = user.UserName;
                dbo.PhoneNumber = user.PhoneNumber;
                dbo.Email = user.Email;
                dbo.Name = user.Name;
                dbo.CitizenId = user.CitizenId;
                dbo.Address = user.Address;
                dbo.ImageUrl = user.ImageUrl;
                _dbContext.AspNetUsers.Update(dbo);
               _dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                error = Utility.GetDataErrror(e);
                return null;
            }
            error.Msg = "Cập nhật thành công";
            return dbo;
        }

        public Account UpdateUser(Guid id, Account user, out DataError error)
        {
            error = new DataError() { Success = true };
            var dbo = _dbContext.Accounts.Find(id);
            try
            {
                BeanUtils.CopyAllPropertySameName(user,dbo);
                _dbContext.Accounts.Update(dbo);
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                error = Utility.GetDataErrror(e);
                error.Success = false;
                return null;
            }
            error.Msg = "Cập nhật thành công";
            return dbo;
        }

        public Account GetUserByEmail(string email)
        {
            var account = new Account();
            try
            {
                account = _dbContext.Accounts.FirstOrDefault(c => c.Email == email);
            }
            catch (Exception e)
            {
                
            }

            return account;
        }

        public Account GetUserByUserName(string userName)
        {
            var account = new Account();
            try
            {
                account = _dbContext.Accounts.FirstOrDefault(c => c.Username == userName);
            }
            catch (Exception e)
            {

            }

            return account;
        }


        public Account GetUserById(Guid idGuid)
        {
            var account = new Account();
            try
            {
                account = _dbContext.Accounts.FirstOrDefault(c => c.Id == idGuid);
            }
            catch (Exception e)
            {

            }

            return account;
        }

        public Account GetUserByPhoneNumber(string phoneNumber)
        {
            var account = new Account();
            try
            {
                account = _dbContext.Accounts.FirstOrDefault(c => c.PhoneNumber == phoneNumber);
            }
            catch (Exception e)
            {

            }

            return account;
        }

        public Account CreateAccountForUser(Account model)
        {
            var account = new Account();
            try
            {
                BeanUtils.CopyAllPropertySameName(model,account);
                 _dbContext.Accounts.Add(account);
                 _dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                account = null;
            }

            return account;
        }
    }
}
