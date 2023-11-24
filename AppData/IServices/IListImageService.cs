using AppData.Models;
using AppData.ViewModels;
using AppData.ViewModels.Phones;

namespace AppData.IServices
{
    public interface IListImageService
    {
        List<ListImage> GetListImagesByIdPhoneDetail(Guid IdPhoneDetail);
        ListImage Create(ListImage model, out DataError error);
        bool Delete(Guid Id);
        int CheckExits(string imageUrl,Guid idPhoneDetail);
        string GetFirstImageByIdPhondDetail(Guid id);
        List<VW_List_By_IdPhone> GetListImageByIdPhone(Guid idPhone);
    }   
}
