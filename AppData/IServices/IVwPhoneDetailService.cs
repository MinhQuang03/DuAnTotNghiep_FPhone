using AppData.Models;
using AppData.ViewModels.Options;
using AppData.ViewModels.Phones;

namespace AppData.IServices
{
    public interface IVwPhoneDetailService
    {
        List<VW_PhoneDetail> listVwPhoneDetails(VW_PhoneDetail model);
        List<VW_PhoneDetail> listVwPhoneDetails(VW_PhoneDetail model,ListOptions options);
      List<PhoneDetaild> listPhoneDetailByIDPhone(Guid id);
        List<VW_PhoneDetail> getListPhoneDetailByIdPhone(Guid idPhone);
        /// <summary>
        /// Lấy chi tiết VW_PhoneDetail 
        /// </summary>
        /// <param name="id">IdPhoneDetail</param>
        /// <returns></returns>
        VW_PhoneDetail getPhoneDetailByIdPhoneDetail(Guid id);
        int CheckPhoneDetail(Guid id);
        Task<PhoneDetaild> Add(PhoneDetaild obj);
    }
}
