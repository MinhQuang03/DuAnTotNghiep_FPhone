using AppData.FPhoneDbContexts;
using AppData.IServices;
using AppData.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PRO219_WebsiteBanDienThoai_FPhone.Models;
using PRO219_WebsiteBanDienThoai_FPhone.Services;
using PRO219_WebsiteBanDienThoai_FPhone.ViewModel;
using PRO219_WebsiteBanDienThoai_FPhone.ViewModel.NewFolder;
using PRO219_WebsiteBanDienThoai_FPhone.ViewModel.Provinces;
 
namespace PRO219_WebsiteBanDienThoai_FPhone.Controllers
{
    public class CheckoutController : Controller
    {
	    private HttpClient _client;
        private IVwPhoneDetailService _service;
        private ICartDetailService _cartDetailService;
        private FPhoneDbContext _context;
        public CheckoutController(HttpClient client,IVwPhoneDetailService service,ICartDetailService cartDetailService)
	    {
		    _client = client;
            _client.DefaultRequestHeaders.Add("token", "a799ced2-febc-11ed-a967-deea53ba3605");
            _service = service;
            _cartDetailService = cartDetailService;
            _context = new FPhoneDbContext();
        }
	    public async Task<IActionResult> Index()
        {
            //user id = user cart
            var userId = User.Claims.FirstOrDefault(claim => claim.Type == "Id")?.Value;
            CheckOutViewModel model = new CheckOutViewModel();
            //lấy dữ liệu tỉnh thành phố
            var data = await (await _client.GetAsync("https://online-gateway.ghn.vn/shiip/public-api/master-data/province")).Content.ReadAsStringAsync();
             var Province = JsonConvert.DeserializeObject<ApiResponse<Province>>(data);
            model.Provinces = Province.Data;
            var car = _context.CartDetails.Where(a => a.IdAccount == (Guid.Parse(userId))).Count();
            if (car > 5)
            {
                TempData["SuccessMessage"] = "Bạn chỉ được mua tối đa 5 sản phẩm !";
                return RedirectToAction("ShowCart","Accounts");
            }
            else if (car == 0)
            {
                TempData["SuccessMessage"] = "Bạn chưa có sản phẩm trong giỏ hàng !";
                return RedirectToAction("ShowCart", "Accounts");
            }

            //kiểm tra đăng nhập
            if (userId == null) //Chưa đăng nhập
                {
                    //lấy thông tin giỏ hàng từ session ( chưa đăng nhập )
                    var datas = SessionService<CartDetailModel>.GetObjFromSession(HttpContext.Session, "Cart");
                    // kiểm tra nếu thông tin trong session đúng với trong db thì gán giá trị cho lstcartdetail
                    foreach (var item in datas)
                    {
                        if (_service.CheckPhoneDetail(item.phoneDetaild.Id) >= 1)
                        {
                            model.lstCartDetail.Add(_service.getPhoneDetailByIdPhoneDetail(item.phoneDetaild.Id));
                        }
                    }
                }

                if (userId != null) //Đã đăng nhập
                {
                    //lấy thông tin giỏ hàng từ database theo IdAccount
                    var datas = _cartDetailService.GetCartDetailsByIdAccount(Guid.Parse(userId));
                    foreach (var item in datas)
                    {
                        if (_service.CheckPhoneDetail(item.IdPhoneDetaild) >= 1)
                        {
                            model.lstCartDetail.Add(_service.getPhoneDetailByIdPhoneDetail(item.IdPhoneDetaild));
                        }
                    }
                }

                return View(model);
        }
        [HttpPost]
        public IActionResult Index(CheckOutViewModel model)
        {
            // lấy data từ model gán vào bill
            model.Bill = new Bill()
            {
                Id = Guid.NewGuid(),
                Address = model.DetailedAddress + ", " + model.Ward + "/" + model.District + "/" + model.Province,
                PaymentDate = DateTime.Now,
                Name = model.Name,
                Status = 0,
                Phone = model.Phone
            };

            return View(model);
        }

        public async Task<JsonResult> GetProvince()
        {
            _client.DefaultRequestHeaders.Add("token", "a799ced2-febc-11ed-a967-deea53ba3605");
	       var data = await _client.GetAsync("https://online-gateway.ghn.vn/shiip/public-api/master-data/province");
	       var name =  data.Content.ReadAsStringAsync();
           return Json(name);
        }
        [HttpGet("/CheckOut/GetVoucherByCode/{maVoucher}")]
        public JsonResult GetVoucherByCode(string maVoucher)
        {
            var voucher = _context.Discount.FirstOrDefault(c =>c.NameVoucher == maVoucher);
            if (voucher != null)
            {
                return Json(voucher);
            }
           return Json(null);
        }
    }
}
