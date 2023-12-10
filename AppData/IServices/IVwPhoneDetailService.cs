using AppData.ViewModels.Options;
using AppData.ViewModels.Phones;

namespace AppData.IServices
{
    public interface IVwPhoneDetailService
    {
        List<VW_PhoneDetail> listVwPhoneDetails(VW_PhoneDetail model);
        List<VW_PhoneDetail> listVwPhoneDetails(VW_PhoneDetail model,ListOptions options);
        List<VW_PhoneDetail> getListPhoneDetailByIdPhone(Guid idPhone); 
        VW_PhoneDetail getPhoneDetailByIdPhoneDetail(Guid id);
        int CheckPhoneDetail(Guid id);  
    }
}
