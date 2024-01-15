using AppData.FPhoneDbContexts;
using AppData.IServices;
using AppData.Utilities;
using AppData.ViewModels.DiscountNguoiDung;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Repositories
{
    public class DiscountNguoiDung : IDiscountNguoiDung
    {
        private readonly FPhoneDbContext _context;
        public DiscountNguoiDung()
        {
            _context = new FPhoneDbContext();
        }
        public Task<List<NguoiDung>> GetLstNguoiDUngMoi()
        {
            throw new NotImplementedException();
        }

        public Task<bool> TangVoucherNguoiDung(AddDiscountRequestDTO addVoucherRequestDTO)
        {
            throw new NotImplementedException();
        }

        public Task<bool> TangVoucherNguoiDungMoi(string ma, string idUser)
        {
            throw new NotImplementedException();
        }
    }
}
