using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModels.DiscountNguoiDung
{
    public class AddDiscountRequestDTO
    {
        public string MaVoucher { get; set; }
        public List<string> UserId { get; set; }
    }
}
