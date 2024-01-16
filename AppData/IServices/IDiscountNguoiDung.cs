using AppData.Utilities;
using AppData.ViewModels.DiscountNguoiDung;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.IServices
{
    public interface IDiscountNguoiDung
    {
        Task<bool> TangVoucherNguoiDung(AddDiscountRequestDTO addVoucherRequestDTO);
        Task<bool> TangVoucherNguoiDungMoi(string ma, string idUser);

        Task<List<NguoiDung>> GetLstNguoiDUngMoi();
    }
}
