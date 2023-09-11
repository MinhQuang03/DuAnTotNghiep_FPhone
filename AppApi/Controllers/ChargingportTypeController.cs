using AppData.IRepositories;
using AppData.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChargingportTypeController : ControllerBase
    {
        private IChargingportTypeRepository _chargingportTypeRepository;
        public ChargingportTypeController(IChargingportTypeRepository chargingportTypeRepository)
        {
            _chargingportTypeRepository = chargingportTypeRepository;
        }

        // GET: api/<ChargingportTypeController>
        [HttpGet("get")]
        public async Task<IActionResult> GetAll()
        {
            var a = await _chargingportTypeRepository.GetAll();
            return Ok(a);
        }

        // GET api/<ChargingportTypeController>/5
        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var a = await _chargingportTypeRepository.GetById(id);
            return Ok(a);
        }

        // POST api/<ChargingportTypeController>
        [HttpPost("add")]
        public async Task<IActionResult> Post(ChargingportType obj)
        {
            var a = await _chargingportTypeRepository.Add(obj);
            return Ok(a);
        }

        // PUT api/<ChargingportTypeController>/5
        [HttpPut("update")]
        public async Task<IActionResult> Put(ChargingportType obj)
        {
            var a = await _chargingportTypeRepository.Update(obj);
            return Ok(a);
        }

        // DELETE api/<ChargingportTypeController>/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _chargingportTypeRepository.Delete(id);
            return Ok();
        }
    }
}
