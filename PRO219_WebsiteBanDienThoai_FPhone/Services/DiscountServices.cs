using AppData.IServices;
using AppData.Models;
using AppData.ViewModels.Discount;
using PRO219_WebsiteBanDienThoai_FPhone.IServices;
using System.Net.Http;

namespace PRO219_WebsiteBanDienThoai_FPhone.Services
{
    public class DiscountServices : _IDiscountServices
    {
        private readonly HttpClient _httpClient;

        public DiscountServices(HttpClient httpClient)
        {
            _httpClient = new HttpClient();
        }
        public async Task<bool> DeleteVoucherWithList(List<string> Id)
        {
            try
            {
                foreach (string item in Id)
                {
                    var response = await _httpClient.PutAsync($"/api/Discount/DeleteVoucher/{item}", null);
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Lỗi xảy ra: {e}");
                return false;
            }
        }

        public async Task<Discount> GetVoucherByMa(string ma)
        {
            return await _httpClient.GetFromJsonAsync<Discount>($"/api/Discount/GetVoucherByMa/{ma}");
        }

        public Task<DiscountDTO> GetVoucherDTOById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> RestoreVoucherWithList(List<string> Id)
        {
            try
            {
                foreach (string item in Id)
                {
                    var response = await _httpClient.PutAsync($"/api/Discount/RestoreVoucher/{item}", null);
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Lỗi xảy ra: {e}");
                return false;
            }
        }

        public async Task<bool> UpdateVoucherAfterUseIt(string idVoucher, string IdNguoiDung)
        {
            try
            {
                var reponse = await _httpClient.PutAsync($"api/Discount/UpdateVoucherAfterUseIt/{idVoucher}/{IdNguoiDung}", null);
                if (reponse.IsSuccessStatusCode)
                {
                    return await reponse.Content.ReadAsAsync<bool>();
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Lỗi xảy ra: {e}");
                return false;
            }
        }

        public async Task<bool> UpdateVoucherSoluong(string idVoucher)
        {
            try
            {
                var reponse = await _httpClient.PutAsync($"https://localhost:7038/api/Voucher/UpdateVoucher/{idVoucher}", null);
                if (reponse.IsSuccessStatusCode)
                {
                    return await reponse.Content.ReadAsAsync<bool>();
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Lỗi xảy ra: {e}");
                return false;
            }
        }
    }
}
