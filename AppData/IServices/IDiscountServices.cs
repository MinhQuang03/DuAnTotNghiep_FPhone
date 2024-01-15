using AppData.ViewModels.Discount;
using AppData.ViewModels.DiscountNguoiDung;
using AppData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.IServices
{
    public interface IDiscountServices
    {
        Task<bool> AddVoucher(Discount discount);
        Task<bool> DeleteVoucher(Guid id);
        Task<bool> EditVoucher(Discount discount);
        Task<List<Discount>> GetallVoucher();
       List<Discount> GetallVoucher2();
      
    }
}
