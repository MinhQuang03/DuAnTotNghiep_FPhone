using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppApi.Controllers
{
    [Route("api/battery")]
    [ApiController]
    public class BatteryController : ControllerBase
    {
        private IBatteryRepository _batteryRepository;
        public BatteryController(IBatteryRepository batteryRepository)
        {
            _batteryRepository = batteryRepository;
        }

        [HttpGet("get")]
        public async Task<IActionResult> Get()
        {

            var a = await _batteryRepository.GetAll();
            return Ok(a);
        }

        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var a = await _batteryRepository.GetById(id);
            return Ok(a);
        }

        // POST api/<ProductionCompanyController>
        [HttpPost("add")]
        public async Task<IActionResult> Post(Battery obj)
        {
            var a = await _batteryRepository.Add(obj);
            return Ok(a);
        }

        // PUT api/<ProductionCompanyController>/5
        [HttpPut("update")]
        public async Task<IActionResult> Put(Battery obj)
        {
            var a = await _batteryRepository.Update(obj);
            return Ok(a);
        }

        // DELETE api/<ProductionCompanyController>/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _batteryRepository.Delete(id);
            return Ok();
        }
    }
}
