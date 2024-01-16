using AppData.FPhoneDbContexts;
using AppData.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Filters;
using X.PagedList;

namespace PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AuthenFilter]
    public class CommentController : Controller
    {
        public FPhoneDbContext _dbContext;
        public CommentController()
        {
            _dbContext = new FPhoneDbContext();
        }
        public IActionResult Index(int? pageNumber, int pageSize = 10, string Id = "")
        {
            
            var bl = _dbContext.Reviews.Where(p => p.IdPhone.ToString() == Id).OrderByDescending(b => b.DateTime).ToList();
            if (bl != null && bl.Any())
            {
                return View(bl.ToPagedList(pageNumber ?? 1, pageSize));
            }
            else
            {
                return View(new List<Review>().ToPagedList(pageNumber ?? 1, pageSize));
            }
        }

        public IActionResult DeleteComment(Guid Id)
        {
            var a = _dbContext.Reviews.FirstOrDefault(p => p.Id == Id);
            _dbContext.Reviews.Remove(a);
            _dbContext.SaveChanges();
            TempData["SuccessMessage"] = "Xóa sản phẩm thành công!";
            return RedirectToAction("Index", new { Id = Id });
        }
    }
}
