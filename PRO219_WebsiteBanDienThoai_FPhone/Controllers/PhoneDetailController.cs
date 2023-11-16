﻿using AppData.FPhoneDbContexts;
using AppData.IRepositories;
using AppData.IServices;
using AppData.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PRO219_WebsiteBanDienThoai_FPhone.Models;
using PRO219_WebsiteBanDienThoai_FPhone.Services;
using PRO219_WebsiteBanDienThoai_FPhone.ViewModel;

namespace PRO219_WebsiteBanDienThoai_FPhone.Controllers
{
    public class PhoneDetailController : Controller
    {
        private HttpClient _client;
        private IVwPhoneDetailService _phoneDetailService;
        private IListImageService _imageService;
        private IPhoneRepository _phoneRepo;
        private FPhoneDbContext _context;
        public PhoneDetailController(HttpClient client,IVwPhoneDetailService phoneDetailService,IListImageService ImageService, IPhoneRepository phoneRepo)
        {
            _context = new FPhoneDbContext();
           _client = client;
            _phoneDetailService = phoneDetailService;
            _imageService = ImageService;
            _phoneRepo = phoneRepo;
        }
        public ActionResult PhoneDetail(string id)
        {
            var sl = 0;

            var userId = User.Claims.FirstOrDefault(claim => claim.Type == "Id")?.Value;
            if (userId == null)
            {
                var product = SessionCartDetail.GetObjFromSession(HttpContext.Session, "Cart");
                ViewBag.sl = product.Count;
            }
            else
            {
                var Cart = _context.CartDetails.Where(a => a.IdAccount == (Guid.Parse(userId))).ToList();
                ViewBag.sl = Cart.Count;
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }
            var data = new VwProductDetailViewModel()
            {
                Records = _phoneDetailService.getListPhoneDetailByIdPhone(Guid.Parse(id)),
                lstImage = null,
                Image = _phoneRepo.GetById(Guid.Parse(id)).Result.Image
            };
            return View(data);
        }
        [HttpGet]
        public ActionResult GetDetailPhones(string id)
        {
            var data = new VwProductDetailViewModel();
            data.Records = _phoneDetailService.getListPhoneDetailByIdPhone(Guid.Parse(id));

            return Json(new
            {
                Items = data.Records,
                Success = true
            });
        }
    }
}
