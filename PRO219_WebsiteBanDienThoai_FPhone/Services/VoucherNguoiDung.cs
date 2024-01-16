using AppData.FPhoneDbContexts;
using AppData.Models;
using AppData.ViewModels.DiscountNguoiDung;
using PRO219_WebsiteBanDienThoai_FPhone.IServices;
using System.Data.Entity;

namespace PRO219_WebsiteBanDienThoai_FPhone.Services
{
    public class VoucherNguoiDung : IVoucherNguoiDung
    {
        private readonly HttpClient _httpClient;
        FPhoneDbContext DbContextModel = new FPhoneDbContext();
        DbSet<VoucherNguoiDung> voucherNguoiDung;
        DbSet<Discount> voucher;
        public VoucherNguoiDung(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public Task<bool> AddVoucherNguoiDung(string MaVoucher, string idNguoiDung)
        {
            throw new NotImplementedException();
        }

        public Task<string> AddVoucherNguoiDungTuAdmin(AddDiscountRequestDTO addVoucherRequestDTO)
        {
            throw new NotImplementedException();
        }

        public bool CheckVoucherInUser(string ma, string idUser)
        {
            throw new NotImplementedException();
        }

        public Task<List<DiscountNguoiDungDTO>> GetAllVouCherNguoiDung()
        {
            throw new NotImplementedException();
        }

        public Task<List<DiscountNguoiDungDTO>> GetAllVoucherNguoiDungByID(string id)
        {
            throw new NotImplementedException();
        }

        public Task<VoucherNguoiDung> GetVoucherNguoiDungById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> TangVoucherNguoiDungMoi(string ma)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateVoucherNguoiDungSauKhiDung(DiscountNguoiDungDTO VcDTO)
        {
            throw new NotImplementedException();
        }
    }
}
