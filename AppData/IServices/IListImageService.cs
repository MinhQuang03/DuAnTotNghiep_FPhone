using AppData.Models;
using AppData.ViewModels;

namespace AppData.IServices
{
    public interface IListImageService
    {
        List<ListImage> GetListImagesByIdPhoneDetail(Guid IdPhoneDetail);
        ListImage Create(ListImage model, out DataError error);
        bool Delete(Guid Id);
        int CheckExits(string imageUrl,Guid idPhoneDetail);

    }   
}
