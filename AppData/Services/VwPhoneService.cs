using AppData.FPhoneDbContexts;
using AppData.IServices;
using AppData.Models;
using AppData.ViewModels.Options;
using AppData.ViewModels.Phones;

namespace AppData.Services
{
    public class VwPhoneService : IVwPhoneService
    {
        private FPhoneDbContext _dbContext;

        public VwPhoneService(FPhoneDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<VW_Phone_Group> listVwPhoneGroup(VW_Phone_Group model)
        {
            
            var lst = new List<VW_Phone_Group>();
            try
            {
                lst = _dbContext.VW_Phone_Group.Where(c =>
                    model == null ||
                    (model.Price==null || c.Price.Contains(model.Price))&&
                    (model.PhoneName == null||c.PhoneName.Contains(model.Price))&&
                    (model.PriceMax == null || c.PriceMax == model.PriceMax)&&
                    (model.ProductionComanyName == null || c.ProductionComanyName.Contains(model.ProductionComanyName)) &&
                    (model.RamName == null || c.RamName.Contains(model.RamName))&&
                    (model.Image == null || c.Image.Contains(model.Image))
                ).ToList();
            }
            catch (Exception e)
            {
               
            }
            return lst;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public List<VW_Phone_Group> listVwPhoneGroup(VW_Phone_Group model, ListOptions options)
        {
            var lst = new List<VW_Phone_Group>();
            try
            {
                lst = _dbContext.VW_Phone_Group.Where(c =>
                    model == null ||
                    (model.Price == null || c.Price.Contains(model.Price)) &&
                    (model.PhoneName == null || c.PhoneName.Contains(model.PhoneName)) &&
                    (model.PriceMax == null || c.PriceMax <= model.PriceMax)
                ).OrderByDescending(c =>c.CreateDate).Skip(options.SkipCalc).Take(options.PageSize).ToList();

                options.AllRecordCount = _dbContext.VW_Phone_Group.Count(c =>
                    model == null ||
                    (model.Price == null || c.Price.Contains(model.Price)) &&
                    (model.PhoneName == null || c.PhoneName.Contains(model.Price)) &&
                    (model.PriceMax == null || c.PriceMax <= model.PriceMax)
                );
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return lst;

        }

        public List<Warranty> ListWarrty()
        {
            var lst = new List<Warranty>();
            try
            {
                lst = _dbContext.Warranty.ToList();
            }
            catch (Exception e)
            {
               
            }

            return lst;
        }

        public List<ProductionCompany> ListCompany()
        {
            var lst = new List<ProductionCompany>();
            try
            {
                lst = _dbContext.ProductionCompany.ToList();
            }
            catch (Exception e)
            {

            }

            return lst;
        }
    }
}
