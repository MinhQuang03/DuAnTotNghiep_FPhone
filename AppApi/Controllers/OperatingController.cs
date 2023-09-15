using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppApi.Controllers
{
    [Route("api/Operating")]
    [ApiController]
    public class OperatingController : ControllerBase
    {
        private IOpertingRepository _opertingRepository;
        public OperatingController(IOpertingRepository OpertingRepository)
        {
            _opertingRepository = OpertingRepository;
        }
        [HttpGet("get")]
        public async Task<IActionResult> Get()
        {

            var a = await _opertingRepository.GetAll();
            return Ok(a);
        }

        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var a = await _opertingRepository.GetById(id);
            return Ok(a);
        }

        // POST api/<ProductionCompanyController>
        [HttpPost("add")]
        public async Task<IActionResult> Post(OperatingSystems obj)
        {
            var a = await _opertingRepository.Add(obj);
            return Ok(a);
        }

        // PUT api/<ProductionCompanyController>/5
        [HttpPut("update")]
        public async Task<IActionResult> Put(OperatingSystems obj)
        {
            var a = await _opertingRepository.Update(obj);
            return Ok(a);
        }

        // DELETE api/<ProductionCompanyController>/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _opertingRepository.Delete(id);
            return Ok();
        }
    }
}
