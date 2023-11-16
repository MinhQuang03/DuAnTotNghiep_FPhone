using AppData.ViewModels.Phones;

namespace AppData.IServices
{
    public interface IVwPhoneDetailService
    {
        List<VW_PhoneDetail> listVwPhoneDetails();
        List<VW_PhoneDetail> getListPhoneDetailByIdPhone(Guid idPhone); 
        VW_PhoneDetail getPhoneDetailByIdPhoneDetail(Guid id);
        int CheckPhoneDetail(Guid id);  
    }
}
