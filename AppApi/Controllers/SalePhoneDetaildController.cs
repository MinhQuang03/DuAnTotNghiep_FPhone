using AppData.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace AppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalePhoneDetaildController : Controller
    {
        private ISalePhoneDetaildRepository _salePhoneDetaildRepository;
        public SalePhoneDetaildController(ISalePhoneDetaildRepository salePhoneDetaildRepository)
        {
            _salePhoneDetaildRepository = salePhoneDetaildRepository;
        }
        [HttpGet("get")]
        public async Task<IActionResult> Get()
        {

            var a = await _salePhoneDetaildRepository.GetAll();
            return Ok(a);
        }
        [HttpPost("add")]
        public async Task<IActionResult> Post(Guid idsp, Guid sale)
        {
            if (await _salePhoneDetaildRepository.Add(idsp, sale))
            {
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
