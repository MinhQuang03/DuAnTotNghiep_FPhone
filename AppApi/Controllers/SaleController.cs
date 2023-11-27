using AppData.IRepositories;
using AppData.Models;
using Microsoft.AspNetCore.Mvc;

namespace AppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController : Controller
    {
        private ISaleRepository _saleRepository;
        public SaleController(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }
        // GET: api/<SalesController>


        // GET api/<SalesController>/5
        [HttpGet("get")]
        public async Task<IActionResult> Get()
        {

            var a = await _saleRepository.GetAll();
            return Ok(a);
        }


        // POST api/<SalesController>

        // PUT api/<SalesController>/5
        [HttpPost("add")]
        public async Task<IActionResult> Post(Sales obj)
        {
            var a = await _saleRepository.Add(obj);
            return Ok(a);
        }
        [HttpPut("update")]
        public async Task<IActionResult> Put(Sales obj)
        {
            var a = await _saleRepository.Update(obj);
            return Ok(a);
        }
        // DELETE api/<SalesController>/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _saleRepository.Delete(id);
            return Ok();
        }
    }
}
