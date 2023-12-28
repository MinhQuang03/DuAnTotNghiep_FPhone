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
using AppData.ViewModels.Phones;
using Microsoft.EntityFrameworkCore;
using Utility = Microsoft.IdentityModel.Tokens.Utility;

namespace AppData.Services
{
    public class ListImageService : IListImageService
    {
        private FPhoneDbContext _dbContext;

        public ListImageService(FPhoneDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<ListImage> GetListImagesByIdPhoneDetail(Guid IdPhoneDetail)
        {
            var lst = new List<ListImage>();
            try
            {
                lst = _dbContext.ListImage.AsNoTracking().Where(c => c.IdPhoneDetaild == IdPhoneDetail).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return lst;
        }

        public ListImage Create(ListImage model, out DataError error)
        {
            error = new DataError() { Success = true };
            try
            {
                _dbContext.ListImage.Add(model);
                _dbContext.SaveChanges();
                error.Msg = "Thêm mới thành công";
                return model;
            }
            catch (Exception e)
            {
                error = Utilities.Utility.GetDataErrror(e);
                return null;
            }
            
          
        }

        public bool Delete(Guid Id)
        {
            try
            {
                var detail = _dbContext.ListImage.FirstOrDefault(c => c.Id == Id);
                _dbContext.Remove(detail);
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                return false;
            }

            return false;
        }

        public int CheckExits(string imageUrl, Guid idPhoneDetail)
        {
            return _dbContext.ListImage.Count(c => c.Image == imageUrl && c.IdPhoneDetaild == idPhoneDetail);
        }

        public string GetFirstImageByIdPhondDetail(Guid id)
        {
            ListImage img = new ListImage();
            try
            {
                img.Image = _dbContext.ListImage.FirstOrDefault(c => c.IdPhoneDetaild == id).Image;
            }
            catch (Exception e)
            {
                img.Image = "";
            }

            return img.Image;
        }

        public List<VW_List_By_IdPhone> GetListImageByIdPhone(Guid idPhone)
        {
            var lst  = new List<VW_List_By_IdPhone>();
            try
            {
                return _dbContext.VW_List_By_IdPhone.Where(c => c.IdPhone == idPhone).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return lst;
        }
    }
}
