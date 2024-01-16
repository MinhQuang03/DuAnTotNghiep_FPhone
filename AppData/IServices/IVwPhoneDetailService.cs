using AppData.Models;
using AppData.ViewModels.Options;
using AppData.ViewModels.Phones;

namespace AppData.IServices
{
    public interface IVwPhoneDetailService
    {
        List<VW_PhoneDetail> listVwPhoneDetails(VW_PhoneDetail model);
        List<VW_PhoneDetail> listVwPhoneDetails(VW_PhoneDetail model,ListOptions options);
        public List<VW_PhoneDetail> listVwPhoneDetails2(VW_PhoneDetail model, ListOptions options);
      List<PhoneDetaild> listPhoneDetailByIDPhone(Guid id);
        List<VW_PhoneDetail> getListPhoneDetailByIdPhone(Guid idPhone);
        /// <summary>
        /// Lấy chi tiết VW_PhoneDetail 
        /// </summary>
        /// <param name="id">IdPhoneDetail</param>
        /// <returns></returns>
        VW_PhoneDetail getPhoneDetailByIdPhoneDetail(Guid id);

        public PhoneDetaild getPhoneDetailById(Guid id);
        int CheckPhoneDetail(Guid id);
        /// <summary>
        /// đếm số lượng imei ( số lượng sản phẩm chưa bán )
        /// </summary>
        /// <param name="idPhoneDetail"></param>
        /// <returns></returns>
        public int CountPhoneDetailFromImei(Guid idPhoneDetail);
        public List<string> GetListImagebyIdPhoneDetail(Guid id);
        public List<Review> GetListComment(string id);
        public int CreateComment(string comment, string idAccount, string idPhone);
     PhoneDetaild Add(PhoneDetaild obj);
     public PhoneDetaild Update(PhoneDetaild obj);
    }
}
