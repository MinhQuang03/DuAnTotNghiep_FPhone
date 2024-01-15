using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Utilities
{
    public class NguoiDung: IdentityUser<String>
    {
        public string? MaNguoiDung { get; set; }
        public string? TenNguoiDung { get; set; }
        public int? GioiTinh { get; set; }
        public string? DiaChi { get; set; }
        public DateTime? NgaySinh { get; set; }
        public double? TongChiTieu { get; set; } = 0;
        public int? SuaDoi { get; set; } = 1;
        public string? AnhDaiDien { get; set; }
        public int? TrangThai { get; set; }
    }
}
