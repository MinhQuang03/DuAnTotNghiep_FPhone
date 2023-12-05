using AppData.IRepositories;
using AppData.Models;
using Microsoft.AspNetCore.Mvc;

namespace AppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalePhoneDetaildController : Controller
    {
        private ISalePhoneDetaildRepository _salePhoneDetaildRepository;
        private IPhoneDetaildRepository _phoneDetaild;
        private ISaleRepository _sale;
        public SalePhoneDetaildController(ISalePhoneDetaildRepository salePhoneDetaildRepository, IPhoneDetaildRepository phoneDetaild, ISaleRepository saleRepository)
        {
            _sale = saleRepository;
            _phoneDetaild = phoneDetaild;
            _salePhoneDetaildRepository = salePhoneDetaildRepository;
        }
        [HttpGet("get")]
        public async Task<IActionResult> Get()
        {

            var a = await _salePhoneDetaildRepository.GetAll();
            return Ok(a);
        }
        [HttpPost("add")]
        public async Task<IActionResult> Post(Guid idphonedetail, Guid idSale)
        {
            if (await _salePhoneDetaildRepository.Add(idphonedetail, idSale) == true)
            {
                var phondetaill = (await _phoneDetaild.GetAll()).FirstOrDefault(p => p.Id == idphonedetail);
                var sale = (await _sale.GetAll()).FirstOrDefault(p => p.Id == idSale);
                if (phondetaill != null && sale != null)
                {
                    //phondetaill.sale = phondetaill.Price - sale.ReducedAmount;
                    await _phoneDetaild.Update(phondetaill);
                }
                return Ok();
            }
            return Ok();
        }


        // PUT api/<ProductionCompanyController>/5
        [HttpPut("update")]
        public async Task<IActionResult> Put(Guid id, Guid idsp, Guid sale)
        {
            var a = _salePhoneDetaildRepository.Update(id, idsp, sale);
            return Ok(a);
        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _salePhoneDetaildRepository.Delete(id);
            return Ok();
        }
    }
}
