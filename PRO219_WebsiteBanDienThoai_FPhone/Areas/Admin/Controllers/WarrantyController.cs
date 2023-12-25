using AppData.FPhoneDbContexts;
using AppData.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity.Infrastructure;

namespace PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Controllers
{
    public class WarrantyController : Controller
    {
        public FPhoneDbContext _dbContext;
        public WarrantyController()
        {
            _dbContext = new FPhoneDbContext();
        }
        public IActionResult Index()
        {
            var a = _dbContext.Warranty.ToList();
            return View(a);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Warranty obj)
        {
            if (ModelState.IsValid)
            {
                await _dbContext.Warranty.AddAsync(obj);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(obj);
        }

        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var warranty = _dbContext.Warranty.Find(id);
            if (warranty == null)
            {
                return NotFound();
            }

            return View(warranty);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, Warranty obj)
        {
            if (id != obj.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _dbContext.Update(obj);
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WarrantyExists(obj.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(obj);
        }

        private bool WarrantyExists(Guid id)
        {
            throw new NotImplementedException();
        }

        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var warranty = _dbContext.Warranty.FirstOrDefault(m => m.Id == id);
            if (warranty == null)
            {
                return NotFound();
            }

            return View(warranty);
        }

        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var warranty = _dbContext.Warranty.FirstOrDefault(m => m.Id == id);
            if (warranty == null)
            {
                return NotFound();
            }

            return View(warranty);
        }

    }
}
