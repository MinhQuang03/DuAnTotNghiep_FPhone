using AppData.ViewModels.Phones;

namespace AppData.IServices
{
    public interface IVwPhoneDetailService
    {
        List<VW_PhoneDetail> listVwPhoneDetails();
        List<VW_PhoneDetail> getListPhoneDetailByIdPhone(Guid idPhone); 
    }
}
