using AppData.FPhoneDbContexts;
using AppData.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using PRO219_WebsiteBanDienThoai_FPhone.Services;
using PRO219_WebsiteBanDienThoai_FPhone.ViewModel;

namespace PRO219_WebsiteBanDienThoai_FPhone.Controllers
{
    public class FavoritePhoneController : Controller
    {
        
        FPhoneDbContext _dbContext = new FPhoneDbContext();
        public async Task<IActionResult> Index()
        {
            var favorite = SessionFavoritePhone.GetObjFromSession(HttpContext.Session, "Index");
            return View(favorite);
        }

        public async Task<IActionResult> Additem(Guid id)
        {
            var item = new FavoritePhoneVM();
            PhoneDetaild product = await _dbContext.PhoneDetailds.FindAsync(id);
            var favorite = SessionFavoritePhone.GetObjFromSession(HttpContext.Session, "Index");
            if(favorite != null)
            {
                var list = (List<FavoritePhoneVM>)favorite;
                var existingItem = list.FirstOrDefault(f => f.favoriteProduct.Id == id);
                if (existingItem != null)
                {
                    // Trả về JSON với thông báo rằng sản phẩm đã tồn tại trong danh sách yêu thích
                    return Json(new
                    {
                        status = 1,
                        method = "ExistProduct"
                    });
                }
                else
                {
                    // Thêm sản phẩm vào danh sách yêu thích
                    item.favoriteProduct = product;
                    item.status = 2;
                    item.method = "favoriteExist";
                    list.Add(item);

                    // Cập nhật danh sách yêu thích trong Session
                    SessionFavoritePhone.SetobjTojson(HttpContext.Session, list ,"Index");

                    // Trả về JSON với thông tin về sản phẩm đã thêm vào danh sách yêu thích
                    return Json(item);
                }
            }
            else
            {
                // Nếu danh sách yêu thích chưa tồn tại, tạo danh sách mới và thêm sản phẩm vào đó
                item.favoriteProduct = product;
                item.status = 3;
                item.method = "favoriteEmpty";
                var list = new List<FavoritePhoneVM>();
                list.Add(item);

                // Lưu danh sách yêu thích vào Session
                SessionFavoritePhone.SetobjTojson(HttpContext.Session, list, "Index");

                // Trả về JSON với thông tin về sản phẩm đã thêm vào danh sách yêu thích
                return Json(item);
            }
        }

    }
}
