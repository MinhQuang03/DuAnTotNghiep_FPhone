using AppData.Models;
using AppData.ViewModels.Discount;
using AppData.ViewModels.DiscountNguoiDung;
using PRO219_WebsiteBanDienThoai_FPhone.Services;

namespace PRO219_WebsiteBanDienThoai_FPhone.IServices
{
    public interface IVoucherNguoiDung
    {
        Task<List<DiscountNguoiDungDTO>> GetAllVouCherNguoiDung();
        Task<List<DiscountNguoiDungDTO>> GetAllVoucherNguoiDungByID(string id);
        Task<VoucherNguoiDung> GetVoucherNguoiDungById(string id);
        Task<bool> AddVoucherNguoiDung(string MaVoucher, string idNguoiDung);
        Task<string> AddVoucherNguoiDungTuAdmin(AddDiscountRequestDTO addVoucherRequestDTO);
        Task<bool> TangVoucherNguoiDungMoi(string ma);
        Task<bool> UpdateVoucherNguoiDungSauKhiDung(DiscountNguoiDungDTO VcDTO);
       
        bool CheckVoucherInUser(string ma, string idUser);
    }
}
