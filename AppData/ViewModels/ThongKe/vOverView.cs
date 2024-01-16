using AppData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModels.ThongKe
{
    public class vOverView
    {
        
        public decimal DoanhThuThangNay { get; set; }
        public decimal DoanhThuThangTruoc { get; set; }
        public int DanhGiaThangNay { get; set; }
        public int DanhGiaThangTruoc { get; set; }
        public int BillOffThangNay { get; set; }
        public int BillOffThangTruoc { get; set; }
        public int BillOnlThangNay { get; set; }
        public int BillOnlThangTruoc { get; set; }
        public int BillThanhCong { get; set; }
        public int BillThatbai { get; set; }

        public List<BillGanDay> billGanDay {  get; set; }
        public List<OverView2> listData { get; set; } = new();

    }

    public class OverView2
    {
        public string BillStatus { get; set; }
        public int CoutBill { get; set; }
    }
}
