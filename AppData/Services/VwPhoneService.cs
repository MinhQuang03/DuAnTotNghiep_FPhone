using AppData.FPhoneDbContexts;
using AppData.IServices;
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
                Console.WriteLine(e);
                throw;
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
                    (model.PhoneName == null || c.PhoneName.Contains(model.Price)) &&
                    (model.PriceMax == null || c.PriceMax <= model.PriceMax)
                ).Take(options.PageSize).ToList();
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
