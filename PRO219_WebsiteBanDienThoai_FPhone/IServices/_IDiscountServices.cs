using AppData.Models;
using AppData.ViewModels.Discount;

namespace PRO219_WebsiteBanDienThoai_FPhone.IServices
{
    public interface _IDiscountServices
    {
        Task<bool> DeleteVoucherWithList(List<string> Id);
        Task<bool> RestoreVoucherWithList(List<string> Id);
        Task<Discount> GetVoucherByMa(string ma);
        Task<DiscountDTO> GetVoucherDTOById(string id);
        Task<bool> UpdateVoucherAfterUseIt(string idVoucher, string IdNguoiDung);
        Task<bool> UpdateVoucherSoluong(string idVoucher);
    }
}
