﻿using System;
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

namespace AppData.Services
{
    public class AccountService :IAccountService
    {
        private FPhoneDbContext _dbContext;

        public AccountService(FPhoneDbContext dbContext)
        {
            _dbContext = dbContext;
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
    }
}