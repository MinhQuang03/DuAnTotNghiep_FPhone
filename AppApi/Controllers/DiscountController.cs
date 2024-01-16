using AppData.FPhoneDbContexts;
using AppData.IRepositories;
using AppData.IServices;
using AppData.Models;
using AppData.Repositories;
using AppData.ViewModels.Discount;
//using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using static AppData.Utilities.Trangthai;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
       
        private readonly IDiscountServices _discountServices;
        FPhoneDbContext DbContextModel = new FPhoneDbContext();
        DbSet<Discount> vouchers;
        private readonly IAllRepo<Discount> allRepo;
        //private readonly IMapper _mapper;
        public DiscountController(IDiscountServices discountServices)
        {
            _discountServices = discountServices;
            vouchers = DbContextModel.Discount;
            AllRepo<Discount> all = new AllRepo<Discount>(DbContextModel, vouchers);
            allRepo = all;
            //_mapper = mapper;
        }

       
      
        [HttpGet()]
        public async Task<IActionResult> Getall()
        {
            var a = await _discountServices.GetallVoucher();
            return Ok(a);
        }

      
        [HttpPost("add")]
        public async Task<IActionResult> Post(Discount obj)
        {
            var a = await _discountServices.AddVoucher(obj);
            return Ok(a);
        }

        [HttpPut("Edit")]
        public async Task<IActionResult> Put(Discount obj)
        {
            var a = await _discountServices.EditVoucher(obj);
            return Ok(a);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _discountServices.DeleteVoucher(id);
            return Ok();
        }
     
     
    }
}
