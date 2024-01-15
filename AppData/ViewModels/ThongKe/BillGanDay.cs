using AppData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModels.ThongKe
{
    public class BillGanDay
    {
        public long STT { get; set; }
        public string MaHoaDon { get; set; }
        public DateTime NgayTao { get; set; }
        public string? TenNguoiMua { get; set; }
        public decimal GiaTien { get; set; }
        public int TrangThaiGiaoHang { get; set; }
        public int TrangThaiThanhToan { get; set; }

    }
}
